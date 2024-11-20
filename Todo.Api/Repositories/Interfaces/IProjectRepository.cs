using Todo.Core.Entities;

namespace Todo.Api.Repositories.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<Dictionary<Project, ProjectRole>> GetAllSharedProjectsAsync(string userId);
}