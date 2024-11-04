using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Todo.Web.Auth.Models;

namespace Todo.Web.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
    }
    
    public async Task<bool> LoginAsync(LoginRequest request)
    {
        try
        {
            Console.WriteLine("Sending");
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            Console.WriteLine("Response Received");
            
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Login response: {response.StatusCode}, Content: {content}");

            if (!response.IsSuccessStatusCode) return false;
            Console.WriteLine("Logged in");
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (result?.Token == null) return false;
            Console.WriteLine($"Token: {result.Token}");
            await _localStorage.SetItemAsync("authToken", result.Token);
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                .MarkUserAsAuthenticated(result.Token);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result?.Token == null) return false;

        await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result.Token);
        return true;
    }


    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrEmpty(token);
    }
}