using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Mappers;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Dtos.TodoListDtos;

namespace Todo.Api.Controllers;

[Authorize]
[Route("api/todo-lists")]
public class TodoListsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationRepository _authorizationRepository;

    public TodoListsController(IUnitOfWork unitOfWork, IAuthorizationRepository authorizationRepository)
    {
        _unitOfWork = unitOfWork;
        _authorizationRepository = authorizationRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodoLists()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();

        var todoLists = await _unitOfWork.TodoLists.GetAllAsync(userId);
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

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanAccessProjectAsync(userId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this project");
        }

        return Ok(todoLists.ToListedTodoListDtos());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTodoListById(int id)
    {
        var todoList = await _unitOfWork.TodoLists.GetByIdAsync(id);

        if (todoList == null)
        {
            return NotFound("Todo-list not found");
        }

        var userId = GetCurrentUserId();

        if (!await _authorizationRepository.CanAccessTodoListAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to this todo-list");
        }

        return Ok(todoList.ToTodoListDto());
    }

    [HttpPost("for-project/{projectId:int}")]
    public async Task<IActionResult> CreateTodoList(int projectId, CreateTodoListDto createTodoListDto)
    {
        var projectExists = await _unitOfWork.Projects.ExistsAsync(projectId);

        if (!projectExists)
        {
            return NotFound("Project not found");
        }

        var userId = GetCurrentUserId();
        
        if (!await _authorizationRepository.CanCreateTodoListAsync(userId, projectId))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to create todo-lists for this project");
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
        
        var userId = GetCurrentUserId();
        
        if (!await _authorizationRepository.CanModifyTodoListAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to modify this todo-list");
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

        var userId = GetCurrentUserId();
        
        if (!await _authorizationRepository.CanModifyTodoListAsync(userId, id))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to delete this todo-list");
        }
        
        await _unitOfWork.TodoLists.DeleteAsync(todoList);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}