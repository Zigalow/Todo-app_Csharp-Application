using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Todo.Web.Auth.Models;
using RegisterRequest = Microsoft.AspNetCore.Identity.Data.RegisterRequest;

namespace Todo.Web.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result?.Token == null) return false;

        await ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(result.Token);
        return true;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result?.Token == null) return false;

        await ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(result.Token);
        return true;
    }

    public async Task LogoutAsync()
    {
        await ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrEmpty(token);
    }
}