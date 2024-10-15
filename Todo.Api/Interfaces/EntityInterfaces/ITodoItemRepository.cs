using Todo.Api.Dtos.TodoItemDtos;
using Todo.Core.Entities;

namespace Todo.Api.Interfaces.EntityInterfaces;

public interface ITodoItemRepository : IRepository<TodoItem>
{
    Task<List<TodoItem>?> GetAllTodoItemsForTodoList(int todoListId);
    Task<List<TodoItem>?> GetAllTodoItemsForProject(int projectId);
    Task<TodoItem?> UpdateAsyncRequest(int id, UpdateTodoItemDto todoItemDto);
}