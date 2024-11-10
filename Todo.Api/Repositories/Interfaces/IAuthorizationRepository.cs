namespace Todo.Api.Repositories.Interfaces;

public interface IAuthorizationRepository
{
    Task<bool> CanAccessProjectAsync(string userId, int projectId);
    Task<bool> CanModifyProjectAsync(string userId, int projectId);
    Task<bool> CanAccessTodoListAsync(string userId, int todoListId);
    Task<bool> CanModifyTodoListAsync(string userId, int todoListId);
    Task<bool> CanAccessTodoItemAsync(string userId, int todoItemId);
    Task<bool> CanModifyTodoItemAsync(string userId, int todoItemId);
    Task<bool> CanAccessTodoLabelAsync(string userId, int labelId);
    Task<bool> CanModifyTodoLabelAsync(string userId, int labelId);
}