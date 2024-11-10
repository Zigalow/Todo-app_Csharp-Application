using Todo.Core.Dtos.TodoListDtos;
using Todo.Core.Entities;
using Todo.Web.Services.interfaces;

namespace Todo.Web.Services;

public class TodoListService:ITodoListService
{
    public Task<List<TodoList>> GetAllTodoListsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TodoList?> GetTodoListByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateTodoListAsync(CreateTodoListDto todoListDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTodoListAsync(int id, UpdateTodoListDto todoList)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteTodoListAsync(int id)
    {
        throw new NotImplementedException();
    }
}