using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.Repositories.Interfaces;
using Todo.Core.Entities;

namespace Todo.Api.Repositories;

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

    public override async Task<IEnumerable<Project>> GetAllAsync(string userId)
    {
        return await _dbContext.Projects
            .Where(p => p.AdminId == userId)
            .Include(p => p.Owner)
            .Include(p => p.TodoLists)
            .ThenInclude(tl => tl.Items)
            .ToListAsync();
    }

    public override async Task<Project?> GetByIdAsync(int id)
    {
        return await _dbContext.Projects
            .Include(p => p.Owner)
            .Include(p => p.TodoLists)
            .ThenInclude(tl => tl.Items)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}