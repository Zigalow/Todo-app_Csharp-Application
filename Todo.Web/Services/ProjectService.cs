using Todo.Core.Dtos.ProjectDtos;
using Todo.Web.Services.interfaces;

namespace Todo.Web.Services;

public class ProjectService : IProjectService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(IHttpClientFactory httpClientFactory, ILogger<ProjectService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _logger = logger;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Projects");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get projects. Status: {StatusCode}", response.StatusCode);
                return new List<ProjectDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<ProjectDto>>() ?? new List<ProjectDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get projects");
            return new List<ProjectDto>();
        }
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Projects/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get project {Id}. Status: {StatusCode}", id, response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ProjectDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to get project {Id}", id);
            return null;
        }
    }

    public async Task<bool> CreateProjectAsync(CreateProjectDto projectDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/projects", projectDto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return false;
        }
    }

    public async Task<bool> UpdateProjectAsync(int id, UpdateProjectDto projectDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/{id}", projectDto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Projects/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {Id}", id);
            return false;
        }
    }
}