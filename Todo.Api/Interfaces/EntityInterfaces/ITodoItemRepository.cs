using Todo.Core.Common;
using Todo.Core.Entities;

namespace Todo.Api.Interfaces.EntityInterfaces;

public interface ITodoItemRepository : IRepository<TodoItem>
{
    Task<List<TodoItem>?> GetAllTodoItemsForTodoList(int todoListId);
    Task<List<TodoItem>?> GetAllTodoItemsForProject(int projectId);
    Task<Result<TodoItem>> AttachLabelToItem(int todoItemId, int labelId);
    Task<Result<TodoItem>> DetachLabelFromItem(int todoItemId, int labelId);
}