using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Dtos.TodoListDtos;
using Todo.Api.Interfaces.EntityInterfaces;
using Todo.Core.Entities;

namespace Todo.Api.Repositories.EntityRepositories;

public class TodoListRepository : GenericRepository<TodoList>, ITodoListRepository
{
    private readonly TodoDbContext _dbContext;

    public TodoListRepository(TodoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TodoList>?> GetAllTodoListsForProject(int projectId)
    {
        var projectWithTodoLists = await _dbContext.Projects
            .Where(p => p.Id == projectId)
            .Select(p => new { p.TodoLists })
            .FirstOrDefaultAsync();

        return projectWithTodoLists?.TodoLists.ToList();
    }

    public async Task<TodoList?> UpdateAsyncRequest(int id, UpdateTodoListDto todoListDto)
    {
        var existingTodoList = await _dbContext.TodoLists.FindAsync(id);

        if (existingTodoList == null)
        {
            return null;
        }

        existingTodoList.Name = todoListDto.Name;

        return existingTodoList;
    }
   
}