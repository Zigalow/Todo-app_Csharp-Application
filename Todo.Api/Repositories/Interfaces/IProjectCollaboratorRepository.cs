using Todo.Core.Common;
using Todo.Core.Entities;

namespace Todo.Api.Repositories.Interfaces;

public interface IProjectCollaboratorRepository
{
    Task<IEnumerable<ProjectCollaborator>> GetCollaboratorsFromProjectAsync(int projectId);
    Task<ProjectCollaborator?> GetProjectCollaboratorByIdAsync(int projectId, string userId);
    Task<Result<bool>> AddCollaboratorAsync(int projectId, ProjectCollaborator projectCollaborator);
    Task<ProjectCollaborator> UpdateProjectCollaboratorAsync(ProjectCollaborator projectCollaborator);
    Task<Result<ProjectCollaborator>> RemoveCollaboratorAsync(int projectId, string removeUserId);
}