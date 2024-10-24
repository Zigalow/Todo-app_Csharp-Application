using Todo.Core.Entities;

namespace Todo.Api.Interfaces.EntityInterfaces;

public interface IProjectRepository : IRepository<Project>
{
    // Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId);
    Task<IEnumerable<ApplicationUser>> GetProjectCollaboratorsAsync(int projectId);
    Task AddCollaboratorAsync(int projectId, string userId, string roleId);
    Task RemoveCollaboratorAsync(int projectId, int userId);
}