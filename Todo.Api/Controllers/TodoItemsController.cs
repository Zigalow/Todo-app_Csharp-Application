using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;
using Todo.Core.Dtos.TodoItemDtos;

namespace Todo.Api.Controllers;

[Authorize]
[Route("api/todo-items")]
public class TodoItemsController : BaseApiController
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

        var userId = GetCurrentUserId();

        var todoItems = await _unitOfWork.TodoItems.GetAllAsync(userId);

        return Ok(todoItems.ToListedTodoItemsDtos());
    }

    [HttpGet("by-project/{projectId:int}")]
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

    [HttpGet("by-list/{todoListId:int}")]
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

    [HttpPost("for-list/{todoListId:int}")]
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

        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(id);

        if (todoItem == null)
        {
            return NotFound("Todo item not found");
        }

        todoItem.UpdateTodoItemFromUpdateDto(updateTodoItemDto);

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

    [HttpPut("{todoItemId:int}/attach-label/{labelId:int}")]
    public async Task<IActionResult> AttachLabelToItem(int todoItemId, int labelId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoItemExists = await _unitOfWork.TodoItems.ExistsAsync(todoItemId);

        if (!todoItemExists)
        {
            return NotFound("Todo item not found");
        }

        var labelExists = await _unitOfWork.Labels.ExistsAsync(labelId);

        if (!labelExists)
        {
            return NotFound("Label not found");
        }

        var result = await _unitOfWork.TodoItems.AttachLabelToItem(todoItemId, labelId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        await _unitOfWork.SaveChangesAsync();
        return Ok(result.Value!.ToTodoItemDto());
    }

    [HttpPut("{todoItemId:int}/detach-label/{labelId:int}")]
    public async Task<IActionResult> DetachLabelFromItem(int todoItemId, int labelId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var todoItemExists = await _unitOfWork.TodoItems.ExistsAsync(todoItemId);

        if (!todoItemExists)
        {
            return NotFound("Todo item not found");
        }

        var labelExists = await _unitOfWork.Labels.ExistsAsync(labelId);

        if (!labelExists)
        {
            return NotFound("Label not found");
        }

        var result = await _unitOfWork.TodoItems.DetachLabelFromItem(todoItemId, labelId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        await _unitOfWork.SaveChangesAsync();
        return Ok(result.Value!.ToTodoItemDto());
    }
}