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