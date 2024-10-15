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

    public new async Task<List<TodoList>> GetAllAsync()
    {
        return await _dbContext.TodoLists
            .Include(tl => tl.Items)
            .ToListAsync();
    }

    public new async Task<TodoList?> GetByIdAsync(int id)
    {
        return await _dbContext.TodoLists
            .Include(tl => tl.Items)
            .FirstOrDefaultAsync(tl => tl.Id == id);
    }

    public async Task<List<TodoList>?> GetAllTodoListsForProject(int projectId)
    {
        var projectWithTodoLists = await _dbContext.Projects
            .Where(p => p.Id == projectId)
            .Include(p => p.TodoLists)
            .ThenInclude(tl => tl.Items)
            .FirstOrDefaultAsync();

        return projectWithTodoLists?.TodoLists.ToList();
    }

    public async Task<TodoList?> UpdateAsyncRequest(int id, UpdateTodoListDto todoListDto)
    {
        var existingTodoList = await GetByIdAsync(id);

        if (existingTodoList == null)
        {
            return null;
        }

        existingTodoList.Name = todoListDto.Name;

        return existingTodoList;
    }
}