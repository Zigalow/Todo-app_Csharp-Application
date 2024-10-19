using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Interfaces.EntityInterfaces;
using Todo.Core.Common;
using Todo.Core.Entities;

namespace Todo.Api.Repositories.EntityRepositories;

public class TodoItemRepository : GenericRepository<TodoItem>, ITodoItemRepository
{
    private readonly TodoDbContext _dbContext;

    public TodoItemRepository(TodoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public new async Task<List<TodoItem>> GetAllAsync()
    {
        return await _dbContext.TodoItems
            .Include(i => i.Labels)
            .ToListAsync();
    }

    public async Task<List<TodoItem>?> GetAllTodoItemsForTodoList(int todoListId)
    {
        var todoItemsFromList = await _dbContext.TodoLists
            .Where(tl => tl.Id == todoListId)
            .Include(tl => tl.Items)
            .ThenInclude(i => i.Labels)
            .Select(tl => tl.Items)
            .FirstOrDefaultAsync();

        return todoItemsFromList?.ToList();
    }

    public async Task<List<TodoItem>?> GetAllTodoItemsForProject(int projectId)
    {
        var todoItemsFromProject = await _dbContext.Projects
            .Where(p => p.Id == projectId)
            .Include(p => p.TodoLists)
            .ThenInclude(tl => tl.Items)
            .ThenInclude(i => i.Labels)
            .SelectMany(p => p.TodoLists)
            .Select(tl => new { tl.Items })
            .FirstOrDefaultAsync();

        return todoItemsFromProject?.Items.ToList();
    }

 
}