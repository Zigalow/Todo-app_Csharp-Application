using Todo.Core.Entities;

namespace Todo.Api.Repositories.Interfaces;

public interface ITodoListRepository : IRepository<TodoList>
{
    public Task<List<TodoList>?> GetAllTodoListsForProject(int projectId);
}