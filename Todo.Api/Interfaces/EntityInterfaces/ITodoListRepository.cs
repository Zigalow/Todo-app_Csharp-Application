using Todo.Api.Dtos.TodoListDtos;
using Todo.Core.Entities;

namespace Todo.Api.Interfaces.EntityInterfaces;

public interface ITodoListRepository : IRepository<TodoList>
{
    public Task<List<TodoList>?> GetAllTodoListsForProject(int projectId);
    Task<TodoList?> UpdateAsyncRequest(int id, UpdateTodoListDto todoListDto);
}