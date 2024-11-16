using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Dtos.TodoItemDtos;

namespace Todo.Api.Controllers;

[Authorize]
[Route("api/todo-items")]
public class TodoItemsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationRepository _authorizationRepository;

    public TodoItemsController(IUnitOfWork unitOfWork, IAuthorizationRepository authorizationRepository)
    {
        _unitOfWork = unitOfWork;
        _authorizationRepository = authorizationRepository;
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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanAccessProjectAsync(userId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this project");
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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanAccessTodoListAsync(userId, todoListId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this todo list");
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

        if (todoItem == null)
        {
            return NotFound("Todo item not found");
        }

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanAccessTodoItemAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this todo item");
        }

        return Ok(todoItem.ToTodoItemDto());
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
            return NotFound("Todo-list not found");
        }

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanCreateTodoItemAsync(userId, todoListId))
        {
            return StatusCode(StatusCodes.Status403Forbidden,
                "User does not have access to create todo item for this todo list");
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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanModifyTodoItemAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to modify this todo item");
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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanModifyTodoItemAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to delete this todo item");
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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanModifyTodoItemAsync(userId, todoItemId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to modify this todo item");
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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanModifyTodoItemAsync(userId, todoItemId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to modify this todo item");
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