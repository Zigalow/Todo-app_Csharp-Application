using Todo.Core.Dtos.TodoListDtos;
using Todo.Core.Entities;

namespace Todo.Web.Services.interfaces;

public interface ITodoListService
{
    Task<List<TodoList>> GetAllTodoListsAsync();
    Task<TodoList?> GetTodoListByIdAsync(int id);
    Task<bool> CreateTodoListAsync(CreateTodoListDto todoListDto);
    Task<bool> UpdateTodoListAsync(int id, UpdateTodoListDto todoList);
    Task<bool> DeleteTodoListAsync(int id);
}