using Microsoft.AspNetCore.Mvc;
using Todo.Api.Dtos.TodoItemDtos;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;

namespace Todo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TodoItemsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodoItems()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoItems = await _unitOfWork.TodoItems.GetAllAsync();

        var todoItemsDto = todoItems.Select(todoItem => todoItem.ToTodoItemDto());

        return Ok(todoItems.ToListedTodoItemsDtos());
    }

    [HttpGet]
    [Route("project/{projectId:int}")]
    public async Task<IActionResult> GetAllTodoItemsForProject(int projectId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoItems = await _unitOfWork.TodoItems.GetAllTodoItemsForProject(projectId);

        if (todoItems == null)
        {
            return NotFound("Project not found");
        }

        return Ok(todoItems.ToListedTodoItemsDtos());
    }

    [HttpGet]
    [Route("todo-list/{todoListId:int}")]
    public async Task<IActionResult> GetAllTodoItemsForTodoList(int todoListId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoItems = await _unitOfWork.TodoItems.GetAllTodoItemsForTodoList(todoListId);

        if (todoItems == null)
        {
            return NotFound("TodoList not found");
        }

        return Ok(todoItems.ToListedTodoItemsDtos());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTodoItemById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(id);
        return todoItem == null ? NotFound() : Ok(todoItem.ToTodoItemDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem(int todoListId, CreateTodoItemDto createTodoItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoListExists = await _unitOfWork.TodoLists.ExistsAsync(todoListId);

        if (!todoListExists)
        {
            return NotFound("TodoList not found");
        }

        var todoItem = createTodoItemDto.ToTodoItemFromCreateDto(todoListId);

        await _unitOfWork.TodoItems.AddAsync(todoItem);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoItemById), new { id = todoItem.Id }, todoItem.ToTodoItemDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTodoItem(int id, UpdateTodoItemDto updateTodoItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoItem = await _unitOfWork.TodoItems.UpdateAsyncRequest(id, updateTodoItemDto);

        if (todoItem == null)
        {
            return NotFound("Todo item not found");
        }

        await _unitOfWork.TodoItems.UpdateAsync(todoItem);
        await _unitOfWork.SaveChangesAsync();

        return Ok(todoItem.ToTodoItemDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(id);

        if (todoItem == null)
        {
            return NotFound("Todo item not found");
        }

        await _unitOfWork.TodoItems.DeleteAsync(todoItem);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}