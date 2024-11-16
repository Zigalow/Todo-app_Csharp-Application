using Todo.Web.Auth.Models;

namespace Todo.Web.Auth;

public class UserService: IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserService(IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<UserInfo?> GetUserInfoAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/user/userinfo");

            if (!response.IsSuccessStatusCode) return null;
            
            return await response.Content.ReadFromJsonAsync<UserInfo>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user info: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> UpdatePhoneNumberAsync(string phoneNumber)
    {
        try
        {
            var info = new UserInfo
            {
                PhoneNumber = phoneNumber
            };

            var response = await _httpClient.PostAsJsonAsync("api/user/updatePhoneNumber",  info);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating phone number: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> HasExternalLoginsAsync()
    {
        /*TODO:Implement logic*/
        return false;
    }
}