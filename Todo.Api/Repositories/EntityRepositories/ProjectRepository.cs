using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Dtos.ProjectDtos;
using Todo.Api.Interfaces.EntityInterfaces;
using Todo.Core.Entities;

namespace Todo.Api.Repositories.EntityRepositories;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    private readonly TodoDbContext _dbContext;

    public ProjectRepository(TodoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ApplicationUser>> GetProjectCollaboratorsAsync(int projectId)
    {
        return await _dbContext.ProjectCollaborators
            .Where(c => c.ProjectId == projectId)
            .Select(c => c.ApplicationUser)
            .ToListAsync();
    }

    public async Task AddCollaboratorAsync(int projectId, string userId, string roleId)
    {
        var collaborator = new ProjectCollaborators
        {
            ProjectId = projectId,
            UserId = userId,
            RoleId = roleId
        };

        await _dbContext.ProjectCollaborators.AddAsync(collaborator);
    }

    public async Task RemoveCollaboratorAsync(int projectId, int userId)
    {
        var collaborator = await _dbContext.ProjectCollaborators
            .FirstOrDefaultAsync(c => c.ProjectId == projectId && c.UserId.Equals(userId));

        if (collaborator != null)
        {
            _dbContext.ProjectCollaborators.Remove(collaborator);
        }
    }

    public async Task<Project?> UpdateAsyncRequest(int id, UpdateProjectDto projectDto)
    {
        var existingProject = await _dbContext.Projects.FirstOrDefaultAsync(project => project.Id == id);

        if (existingProject == null)
        {
            return null;
        }

        existingProject.Name = projectDto.Name;

        return existingProject;
    }
}