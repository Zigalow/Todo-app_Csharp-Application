using Todo.Core.Dtos.LabelDtos;
using Todo.Core.Dtos.TodoItemDtos;
using Todo.Web.Services.interfaces;

namespace Todo.Web.Services;

public class LabelService : ILabelService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LabelService> _logger;

    public LabelService(IHttpClientFactory httpClientFactory, ILogger<LabelService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _logger = logger;
    }

    public async Task<List<LabelDto>> GetAllLabelsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Labels");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get labels. Status: {StatusCode}", response.StatusCode);
                return new List<LabelDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<LabelDto>>() ?? new List<LabelDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get labels");
            return new List<LabelDto>();
        }
    }

    public async Task<LabelDto?> GetLabelByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Labels/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get label {Id}. Status: {StatusCode}", id, response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<LabelDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to get label {Id}", id);
            return null;
        }
    }

    public async Task<List<LabelDto?>> GetAllLabelsForProject(int projectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Labels/by-project/{projectId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get labels for project {ProjectId}. Status: {StatusCode}", projectId,
                    response.StatusCode);
                return new List<LabelDto?>();
            }

            return await response.Content.ReadFromJsonAsync<List<LabelDto>>() ?? new List<LabelDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get labels for project {ProjectId}", projectId);
            return null;
        }
    }

    public async Task<bool> CreateLabelAsync(int projectId,CreateLabelDto labelDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Labels/for-project/{projectId}", labelDto);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to create label. Status: {StatusCode}", response.StatusCode);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to create label");
            return false;
        }
    }

    public async Task<bool> UpdateLabelAsync(int id, UpdateLabelDto labelDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Labels/{id}", labelDto);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to update label {Id}. Status: {StatusCode}", id, response.StatusCode);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to update label {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteLabelAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Labels/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to delete label {Id}. Status: {StatusCode}", id, response.StatusCode);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to delete label {Id}", id);
            return false;
        }
    }
}