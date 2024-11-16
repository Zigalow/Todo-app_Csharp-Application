using Todo.Core.Dtos.TodoItemDtos;

namespace Todo.Web.Services.interfaces;

public interface ITodoItemService
{
    Task<List<TodoItemDto>> GetAllTodoItemsAsync();
    Task<List<TodoItemDto>?> GetAllTodoItemsForProject(int projectId);
    Task<List<TodoItemDto>?> GetAllTodoItemsForList(int listId);
    Task<List<TodoItemDto>?> GetTodoItemByIdAsync(int id);
    Task<bool> CreateTodoItemAsync(int listId, CreateTodoItemDto todoItemDto);
    Task<bool> UpdateTodoItemAsync(int id, UpdateTodoItemDto todoItem);
    Task<bool> AttachLabelToItemAsync(int itemId, int labelId);
    Task<bool> DetachLabelFromItemAsync(int itemId, int labelId);
    Task<bool> DeleteTodoItemAsync(int id);
}