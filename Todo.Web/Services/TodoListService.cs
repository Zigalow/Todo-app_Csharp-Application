using Todo.Core.Dtos.TodoListDtos;
using Todo.Web.Services.interfaces;

namespace Todo.Web.Services;

public class TodoListService : ITodoListService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TodoListService> _logger;

    public TodoListService(IHttpClientFactory httpClientFactory, ILogger<TodoListService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _logger = logger;
    }

    public async Task<List<TodoListDto>> GetAllTodoListsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/todo-lists");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get todo-lists. Status: {StatusCode}", response.StatusCode);
                return new List<TodoListDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<TodoListDto>>() ?? new List<TodoListDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get todo-lists");
            return new List<TodoListDto>();
        }
    }

    public async Task<List<TodoListDto>?> GetAllTodoListsForProject(int projectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/todo-lists/by-project/{projectId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get todo-lists for project {ProjectId}. Status: {StatusCode}", projectId,
                    response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<TodoListDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get todo-lists for project {ProjectId}", projectId);
            return null;
        }
    }

    public async Task<List<TodoListDto>?> GetTodoListByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/todo-lists/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get todo-lists for todo-list {Id}. Status: {StatusCode}", id,
                    response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<TodoListDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get todo-lists for todo-lists {id}", id);
            return null;
        }
    }

    public async Task<bool> CreateTodoListAsync(int projectId, CreateTodoListDto todoListDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/todo-lists/for-project/{projectId}", todoListDto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating todo-list for project {ProjectId}", projectId);
            return false;
        }
    }

    public async Task<bool> UpdateTodoListAsync(int id, UpdateTodoListDto todoListDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/todo-lists/{id}", todoListDto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating todo-list {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteTodoListAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/todo-lists/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting todo-list {Id}", id);
            return false;
        }
    }
}