using Todo.Core.Entities;

namespace Todo.Api.Repositories.Interfaces;

public interface ITodoListRepository : IRepository<TodoList>
{
    public Task<List<TodoList>?> GetAllTodoListsForProject(int projectId);

    public Task<List<TodoList>> GetAllTodoListsForUser(String userId);
}