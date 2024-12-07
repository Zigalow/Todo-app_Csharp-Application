using Todo.Core.Dtos.ProjectCollaboratorDtos;
using Todo.Core.Entities;
using Todo.Web.Auth.Models;
using Todo.Web.Services.interfaces;

namespace Todo.Web.Services;

public class ProjectCollaboratorService : IProjectCollaboratorService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectCollaborator> _logger;

    public ProjectCollaboratorService(IHttpClientFactory httpClientFactory, ILogger<ProjectCollaborator> logger)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _logger = logger;
    }

    public async Task<List<ProjectCollaboratorDto>> GetCollaboratorsFromProjectAsync(int projectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/projects/{projectId}/collaborators/collaboratorsFromProject");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get collaborators from project {ProjectId}. Status: {StatusCode}",
                    projectId, response.StatusCode);
                return new List<ProjectCollaboratorDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<ProjectCollaboratorDto>>() ??
                   new List<ProjectCollaboratorDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get collaborators from project {ProjectId}", projectId);
            return new List<ProjectCollaboratorDto>();
        }
    }

    public async Task<ProjectCollaboratorDto?> GetCollaboratorFromProjectAsync(int projectId, string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/projects/{projectId}/collaborators/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get collaborator {UserId} from project {ProjectId}. Status: {StatusCode}",
                    userId, projectId, response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ProjectCollaboratorDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get collaborator {UserId} from project {ProjectId}", userId, projectId);
            return null;
        }
    }

    public async Task<bool> AddCollaboratorToProjectAsync(int projectId,
        AddProjectCollaboratorDto projectCollaboratorDto)
    {
        try
        {
            var response =
                await _httpClient.PostAsJsonAsync($"api/projects/{projectId}/collaborators", projectCollaboratorDto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add collaborator to project {ProjectId}", projectId);
            return false;
        }
    }

    public async Task<bool> UpdateCollaboratorFromProjectAsync(int projectId, string userId,
        UpdateProjectCollaboratorDto projectCollaboratorDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/{projectId}/collaborators/{userId}",
                projectCollaboratorDto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update collaborator {UserId} from project {ProjectId}", userId, projectId);
            return false;
        }
    }

    public async Task<bool> RemoveCollaboratorFromProjectAsync(int projectId, string userId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{projectId}/collaborators/{userId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to remove collaborator {UserId} from project {ProjectId}", userId, projectId);
            return false;
        }
    }

    public async Task<bool> RemoveSelfFromProjectAsync(int projectId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{projectId}/collaborators/self");
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to remove self from project {ProjectId}", projectId);
            return false;
        }
    }
    
    public async Task<ProjectRole?> GetCurrentUserRoleInProjectAsync(int projectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/projects/{projectId}/collaborators/currentUserRole");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get Current Users role for project {ProjectId}. Status: {StatusCode}",
                    projectId, response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ProjectRole>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Current Users role for project {ProjectId}", projectId);
            return null;
        }
    }
}