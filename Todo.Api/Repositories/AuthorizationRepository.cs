using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Entities;

namespace Todo.Api.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly TodoDbContext _dbContext;

    public AuthorizationRepository(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsAdminAsync(string userId, int projectId)
    {
        return await _dbContext.Projects.AnyAsync(p => p.AdminId == userId);
    }

    public async Task<bool> CanAccessProjectAsync(string userId, int projectId)
    {
        if (await IsAdminAsync(userId))
            return true;

        return await _dbContext.Projects
            .AnyAsync(p => p.Id == projectId && p.Collaborators.Any(c => c.UserId == userId));
    }

    public async Task<bool> CanModifyProjectAsync(string userId, int projectId)
    {
        if (await IsAdminAsync(userId))
            return true;

        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProjectId == projectId);

        return collaborator != null && HasPermission(collaborator.Role, Permissions.ManageProject);
    }

    public async Task<bool> CanAccessTodoListAsync(string userId, int todoListId)
    {
        if (await IsAdminAsync(userId))
            return true;

        return await _dbContext.TodoLists
            .AnyAsync(tl => tl.Id == todoListId &&
                            tl.Project.Collaborators.Any(c => c.UserId == userId));
    }

    public async Task<bool> CanCreateTodoListAsync(string userId, int projectId)
    {
        if (await IsAdminAsync(userId))
            return true;

        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProjectId == projectId);

        return collaborator != null && HasPermission(collaborator.Role, Permissions.ManageTodoLists);
    }

    public async Task<bool> CanModifyTodoListAsync(string userId, int todoListId)
    {
        if (await IsAdminAsync(userId))
            return true;

        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c => c.UserId == userId && c.Project.TodoLists.Any(tl => tl.Id == todoListId));

        return collaborator != null && HasPermission(collaborator.Role, Permissions.ManageTodoLists);
    }

    public async Task<bool> CanAccessTodoItemAsync(string userId, int todoItemId)
    {
        if (await IsAdminAsync(userId))
            return true;

        return await _dbContext.TodoItems
            .AnyAsync(ti => ti.Id == todoItemId &&
                            ti.TodoList.Project.Collaborators.Any(c => c.UserId == userId));
    }

    public async Task<bool> CanCreateTodoItemAsync(string userId, int todoListId)
    {
        if (await IsAdminAsync(userId))
            return true;

        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c => c.UserId == userId && c.Project.TodoLists.Any(tl => tl.Id == todoListId));

        return collaborator != null && HasPermission(collaborator.Role, Permissions.ManageTodoItems);
    }

    public async Task<bool> CanModifyTodoItemAsync(string userId, int todoItemId)
    {
        if (await IsAdminAsync(userId))
            return true;

        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c =>
                c.UserId == userId && c.Project.TodoLists.Any(tl => tl.Items.Any(ti => ti.Id == todoItemId)));

        return collaborator != null && HasPermission(collaborator.Role, Permissions.ManageTodoItems);
    }

    public async Task<bool> CanAccessLabelAsync(string userId, int labelId)
    {
        if (await IsAdminAsync(userId))
            return true;

        return await _dbContext.Labels
            .AnyAsync(l => l.Id == labelId && l.Project.Collaborators.Any(c => c.UserId == userId));
    }

    public async Task<bool> CanCreateLabelAsync(string userId, int projectId)
    {
        if (await IsAdminAsync(userId))
            return true;

        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProjectId == projectId);

        return collaborator != null && HasPermission(collaborator.Role, Permissions.ManageLabels);
    }

    public async Task<bool> CanModifyLabelAsync(string userId, int labelId)
    {
        if (await IsAdminAsync(userId))
            return true;

        var projectId = await _dbContext.Labels
            .Where(l => l.Id == labelId)
            .Select(l => l.ProjectId)
            .FirstOrDefaultAsync();

        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProjectId == projectId);

        return collaborator != null && HasPermission(collaborator.Role, Permissions.ManageTodoItems);
    }

    public async Task<bool> CanManageProjectCollaborator(string userId, int projectId)
    {
        if (await IsAdminAsync(userId, projectId))
            return true;

        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProjectId == projectId);

        return collaborator != null && HasPermission(collaborator.Role, Permissions.ManageProjectCollaborators);
    }

    private static bool HasPermission(ProjectRole role, string permission)
    {
        return RolePermissions.HasPermission(role, permission);
    }

    private async Task<int> GetProjectIdFromTodoList(int todoListId)
    {
        return await _dbContext.TodoLists
            .Where(tl => tl.Id == todoListId)
            .Select(tl => tl.ProjectId)
            .FirstOrDefaultAsync();
    }

    private async Task<int> GetProjectIdFromTodoItem(int todoItemId)
    {
        return await _dbContext.TodoItems
            .Where(ti => ti.Id == todoItemId)
            .Select(ti => ti.TodoList.ProjectId)
            .FirstOrDefaultAsync();
    }

    private async Task<int> GetProjectIdFromLabel(int labelId)
    {
        return await _dbContext.Labels
            .Where(l => l.Id == labelId)
            .Select(l => l.ProjectId)
            .FirstOrDefaultAsync();
    }
}