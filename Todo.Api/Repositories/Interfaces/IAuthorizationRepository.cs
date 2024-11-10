namespace Todo.Api.Repositories.Interfaces;

public interface IAuthorizationRepository
{
    Task<bool> CanAccessProjectAsync(string userId, int projectId);
    Task<bool> CanCreateProjectAsync(string userId);
    Task<bool> CanModifyProjectAsync(string userId, int projectId);
    Task<bool> CanAccessTodoListAsync(string userId, int todoListId);
    Task<bool> CanCreateTodoListAsync(string userId, int projectId);
    Task<bool> CanModifyTodoListAsync(string userId, int todoListId);
    Task<bool> CanAccessTodoItemAsync(string userId, int todoItemId);
    Task<bool> CanCreateTodoItemAsync(string userId, int todoListId);
    Task<bool> CanModifyTodoItemAsync(string userId, int todoItemId);
    Task<bool> CanAccessLabelAsync(string userId, int labelId);
    Task<bool> CanCreateLabelAsync(string userId, int projectId);
    Task<bool> CanModifyLabelAsync(string userId, int labelId);
}