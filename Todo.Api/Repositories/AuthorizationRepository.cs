using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Repositories.Interfaces;

namespace Todo.Api.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly TodoDbContext _dbContext;

    public AuthorizationRepository(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CanAccessProjectAsync(string userId, int projectId)
    {
        return await _dbContext.Projects
            .AnyAsync(p => p.Id == projectId &&
                           (p.AdminId == userId || p.Collaborators.Any(c => c.UserId == userId)));
    }

    public Task<bool> CanCreateProjectAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CanModifyProjectAsync(string userId, int projectId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CanAccessTodoListAsync(string userId, int todoListId)
    {
        return await _dbContext.TodoLists
            .AnyAsync(tl => tl.Id == todoListId &&
                            (tl.Project.AdminId == userId ||
                             tl.Project.Collaborators.Any(c => c.UserId == userId)));
    }

    public Task<bool> CanCreateTodoListAsync(string userId, int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CanModifyTodoListAsync(string userId, int todoListId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CanAccessTodoItemAsync(string userId, int todoItemId)
    {
        return await _dbContext.TodoItems
            .AnyAsync(ti => ti.Id == todoItemId &&
                            (ti.TodoList.Project.AdminId == userId ||
                             ti.TodoList.Project.Collaborators.Any(c => c.UserId == userId)));
    }

    public Task<bool> CanCreateTodoItemAsync(string userId, int todoListId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CanModifyTodoItemAsync(string userId, int todoItemId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CanAccessLabelAsync(string userId, int labelId)
    {
        return await _dbContext.Labels
            .AnyAsync(l => l.Id == labelId &&
                           (l.Project.AdminId == userId ||
                            l.Project.Collaborators.Any(c => c.UserId == userId)));
    }

    public Task<bool> CanCreateLabelAsync(string userId, int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CanModifyLabelAsync(string userId, int labelId)
    {
        throw new NotImplementedException();
    }
}