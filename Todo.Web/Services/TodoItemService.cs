using Todo.Core.Dtos.TodoItemDtos;
using Todo.Web.Services.interfaces;

namespace Todo.Web.Services;

public class TodoItemService : ITodoItemService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TodoItemService> _logger;

    public TodoItemService(IHttpClientFactory httpClientFactory, ILogger<TodoItemService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _logger = logger;
    }

    public async Task<List<TodoItemDto>> GetAllTodoItemsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/todo-items");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get todo-items. Status: {StatusCode}", response.StatusCode);
                return new List<TodoItemDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<TodoItemDto>>() ?? new List<TodoItemDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get todo-items");
            return new List<TodoItemDto>();
        }
    }

    public async Task<List<TodoItemDto>?> GetAllTodoItemsForProject(int projectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/todo-items/by-project/{projectId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get todo-items for project {ProjectId}. Status: {StatusCode}", projectId,
                    response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<TodoItemDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get todo-items for project {ProjectId}", projectId);
            return null;
        }
    }

    public async Task<List<TodoItemDto>?> GetAllTodoItemsForList(int listId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/todo-items/by-project/{listId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get todo-items for todo-list {ListId}. Status: {StatusCode}", listId,
                    response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<List<TodoItemDto>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get todo-items for todo-list {ListId}", listId);
            return null;
        }
    }

    public async Task<TodoItemDto?> GetTodoItemByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/todo-items/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get todo-item {Id}. Status: {StatusCode}", id, response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TodoItemDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get todo-item {Id}", id);
            return null;
        }
    }

    public async Task<bool> CreateTodoItemAsync(int listId, CreateTodoItemDto todoItemDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/todo-items/for-list/{listId}", todoItemDto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating todo-item for list {ListId}", listId);
            return false;
        }
    }

    public async Task<bool> UpdateTodoItemAsync(int id, UpdateTodoItemDto todoItem)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/todo-items/{id}", todoItem);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating todo-item {Id}", id);
            return false;
        }
    }

    public async Task<bool> AttachLabelToItemAsync(int itemId, int labelId)
    {
        try
        {
            var response = await _httpClient.PutAsync($"api/todo-items/{itemId}/attach-label/{labelId}", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error attaching label {LabelId} to todo-item {ItemId}", labelId, itemId);
            return false;
        }
    }

    public async Task<bool> DetachLabelFromItemAsync(int itemId, int labelId)
    {
        try
        {
            var response = await _httpClient.PutAsync($"api/todo-items/{itemId}/detach-label/{labelId}", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detaching label {LabelId} from todo-item {ItemId}", labelId, itemId);
            return false;
        }
    }

    public async Task<bool> DeleteTodoItemAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/todo-items/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting todo-item {Id}", id);
            return false;
        }
    }
}