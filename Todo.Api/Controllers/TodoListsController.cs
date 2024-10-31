using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;
using Todo.Core.Dtos.TodoListDtos;

namespace Todo.Api.Controllers;

[Authorize]
[Route("api/todo-lists")]
public class TodoListsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public TodoListsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodoLists()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoLists = await _unitOfWork.TodoLists.GetAllAsync();
        return Ok(todoLists.ToListedTodoListDtos());
    }

    [HttpGet("by-project/{projectId:int}")]
    public async Task<IActionResult> GetAllTodoListsForProject(int projectId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoLists = await _unitOfWork.TodoLists.GetAllTodoListsForProject(projectId);

        if (todoLists == null)
        {
            return NotFound("Project not found");
        }

        return Ok(todoLists.ToListedTodoListDtos());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTodoListById(int id)
    {
        var todoList = await _unitOfWork.TodoLists.GetByIdAsync(id);
        return todoList == null ? NotFound() : Ok(todoList.ToTodoListDto());
    }

    [HttpPost("for-project/{projectId:int}")]
    public async Task<IActionResult> CreateTodoList(int projectId, CreateTodoListDto createTodoListDto)
    {
        var projectExists = await _unitOfWork.Projects.ExistsAsync(projectId);

        if (!projectExists)
        {
            return NotFound("Project not found");
        }

        var todoList = createTodoListDto.ToTodoListFromCreateDto(projectId);

        await _unitOfWork.TodoLists.AddAsync(todoList);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoListById), new { id = todoList.Id }, todoList.ToTodoListDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTodoList(int id, UpdateTodoListDto updateTodoListDto)
    {
        var todoList = await _unitOfWork.TodoLists.GetByIdAsync(id);

        if (todoList == null)
        {
            return NotFound("Todo list not found");
        }

        todoList.UpdateTodoListFromUpdateDto(updateTodoListDto);

        await _unitOfWork.TodoLists.UpdateAsync(todoList);
        await _unitOfWork.SaveChangesAsync();

        return Ok(todoList.ToTodoListDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTodoList(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoList = await _unitOfWork.TodoLists.GetByIdAsync(id);
        if (todoList == null)
        {
            return NotFound("Todo list not found");
        }

        await _unitOfWork.TodoLists.DeleteAsync(todoList);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}