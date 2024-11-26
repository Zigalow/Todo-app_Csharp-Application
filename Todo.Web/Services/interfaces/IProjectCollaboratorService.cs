using Todo.Core.Dtos.ProjectCollaboratorDtos;

namespace Todo.Web.Services.interfaces;

public interface IProjectCollaboratorService
{
    Task<List<ProjectCollaboratorDto>> GetCollaboratorsFromProjectAsync(int projectId);
    Task<ProjectCollaboratorDto?> GetCollaboratorFromProjectAsync(int projectId, string userId);
    Task<bool> AddCollaboratorToProjectAsync(int projectId, AddProjectCollaboratorDto projectCollaboratorDto);

    Task<bool> UpdateCollaboratorFromProjectAsync(int projectId, string userId,
        UpdateProjectCollaboratorDto projectCollaboratorDto);

    Task<bool> RemoveCollaboratorFromProjectAsync(int projectId, string userId);
    
    Task<bool> RemoveSelfFromProjectAsync(int projectId);
}