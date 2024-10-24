using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
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
            .Include(p => p.TodoLists)
            .ThenInclude(tl => tl.Items)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        return projectWithTodoLists?.TodoLists.ToList();
    }
}