using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Entities;

namespace Todo.Api.Repositories;

public class TodoListRepository : GenericRepository<TodoList>, ITodoListRepository
{
    private readonly TodoDbContext _dbContext;

    public TodoListRepository(TodoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<IEnumerable<TodoList>> GetAllAsync(string userId)
    {
        var projectsWithTodoLists = await _dbContext.Projects
            .Include(p => p.TodoLists)
            .ThenInclude(tl => tl.Items)
            .ThenInclude(l=>l.Labels)
            .Where(p => p.AdminId == userId)
            .ToListAsync();

        return projectsWithTodoLists.SelectMany(p => p.TodoLists).ToList();
    }

    public override async Task<TodoList?> GetByIdAsync(int id)
    {
        return await _dbContext.TodoLists
            .Include(tl => tl.Items)
            .ThenInclude(l=>l.Labels)
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