using Todo.Core.Dtos.TodoListDtos;

namespace Todo.Web.Services.interfaces;

public interface ITodoListService
{
    Task<List<TodoListDto>> GetAllTodoListsAsync();
    Task<List<TodoListDto>?> GetAllTodoListsForProject(int projectId);
    Task<List<TodoListDto>?> GetTodoListByIdAsync(int id);
    Task<bool> CreateTodoListAsync(int projectId, CreateTodoListDto todoListDto);
    Task<bool> UpdateTodoListAsync(int id, UpdateTodoListDto todoList);
    Task<bool> DeleteTodoListAsync(int id);
}