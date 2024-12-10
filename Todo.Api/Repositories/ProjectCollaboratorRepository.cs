using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Common;
using Todo.Core.Entities;

namespace Todo.Api.Repositories;

public class ProjectCollaboratorRepository : IProjectCollaboratorRepository
{
    private readonly TodoDbContext _dbContext;

    public ProjectCollaboratorRepository(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProjectCollaborator>> GetCollaboratorsFromProjectAsync(int projectId)
    {
        return await _dbContext.ProjectCollaborators
            .Where(pc => pc.ProjectId == projectId)
            .Include(pc => pc.ApplicationUser)
            .ToListAsync();
    }

    public async Task<ProjectCollaborator?> GetProjectCollaboratorByIdAsync(int projectId, string userId)
    {
        return await _dbContext.ProjectCollaborators
            .Where(pc => pc.ProjectId == projectId && pc.UserId == userId)
            .Include(pc => pc.ApplicationUser)
            .FirstOrDefaultAsync();
    }

    public async Task<Result<bool>> AddCollaboratorAsync(int projectId, ProjectCollaborator projectCollaborator)
    {
        var adminId = await _dbContext.Projects
            .Where(p => p.Id == projectId)
            .Select(p => p.AdminId)
            .FirstOrDefaultAsync();

        if (projectCollaborator.UserId.Equals(adminId))
        {
            return Result<bool>.Forbidden("User is the admin of the project");
        }

        var isExisting = await _dbContext.ProjectCollaborators
            .AnyAsync(c => c.ProjectId == projectId && c.UserId.Equals(projectCollaborator.UserId));

        if (isExisting)
        {
            return Result<bool>.Failure("User is already a part of the project");
        }

        var collaborator = new ProjectCollaborator
        {
            ProjectId = projectId,
            UserId = projectCollaborator.UserId,
            Role = projectCollaborator.Role
        };

        await _dbContext.ProjectCollaborators.AddAsync(collaborator);

        return Result<bool>.Success(true);
    }

    public Task<ProjectCollaborator> UpdateProjectCollaboratorAsync(ProjectCollaborator projectCollaborator)
    {
        _dbContext.ProjectCollaborators.Update(projectCollaborator);

        return Task.FromResult(projectCollaborator);
    }

    public async Task<Result<ProjectCollaborator>> RemoveCollaboratorAsync(int projectId, string removeUserId)
    {
        var adminId = await _dbContext.Projects
            .Where(p => p.Id == projectId)
            .Select(p => p.AdminId)
            .FirstOrDefaultAsync();

        if (removeUserId.Equals(adminId))
        {
            return Result<ProjectCollaborator>.Forbidden("Cannot remove admin from the project");
        }

        // Find the existing collaborator
        var collaborator = await GetProjectCollaboratorByIdAsync(projectId, removeUserId);

        if (collaborator == null)
        {
            return Result<ProjectCollaborator>.Failure("User is not a part of the project");
        }

        _dbContext.ProjectCollaborators.Remove(collaborator);

        return Result<ProjectCollaborator>.Success(collaborator);
    }
}