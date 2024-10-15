using Microsoft.AspNetCore.Mvc;
using Todo.Api.Dtos.TodoListDtos;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;

namespace Todo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoListController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TodoListController(IUnitOfWork unitOfWork)
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
        return Ok(todoLists);
    }

    [HttpGet]
    [Route("project/{projectId:int}")]
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

        return Ok(todoLists);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTodoListById(int id)
    {
        var todoList = await _unitOfWork.TodoLists.GetByIdAsync(id);
        return todoList == null ? NotFound() : Ok(todoList);
    }

    [HttpPost]
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

        return CreatedAtAction(nameof(GetTodoListById), new { id = todoList.Id }, todoList);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTodoList(int id, UpdateTodoListDto updateTodoListDto)
    {
        var todoList = await _unitOfWork.TodoLists.UpdateAsyncRequest(id, updateTodoListDto);

        if (todoList == null)
        {
            return NotFound("Todo list not found");
        }

        await _unitOfWork.TodoLists.UpdateAsync(todoList);
        await _unitOfWork.SaveChangesAsync();

        return Ok(todoList);
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