using Todo.Core.Dtos.ProjectDtos;

namespace Todo.Web.Services.interfaces;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<bool> CreateProjectAsync(CreateProjectDto projectDto);
    Task<bool> UpdateProjectAsync(int id, UpdateProjectDto project);
    Task<bool> DeleteProjectAsync(int id);
 
}