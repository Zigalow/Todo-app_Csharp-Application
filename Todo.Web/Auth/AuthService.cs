using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Todo.Web.Auth.Models;

namespace Todo.Web.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _authenticationStateProvider = authenticationStateProvider;
        _httpContextAccessor = httpContextAccessor;

    }

    public string? GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (result?.Token == null) return false;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(result.Token);
            var claims = jwt.Claims.ToList();

            // Add the JWT token as a claim
            claims.Add(new Claim("jwt_token", result.Token));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user
            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                });

            ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                .NotifyUserAuthentication(principal);

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
        try
        {
            var response = await _httpClient.PostAsync("api/auth/logout", null);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Backend logout failed");
            }

            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logout error: {ex.Message}");
        }
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return AuthResult.Success(content);
            }

            return AuthResult.Failure(!string.IsNullOrWhiteSpace(content)
                ? content
                : "Registration failed. Please try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Register error: {ex.Message}");
            return AuthResult.Failure("Registration failed. Please try again.");
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<AuthResult> ResendConfirmationEmailAsync(string email)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/resend-confirmation-email", new { email });

            if (response.IsSuccessStatusCode)
            {
                return AuthResult.Success("A new confirmation email has been sent.");
            }

            var error = await response.Content.ReadAsStringAsync();
            return AuthResult.Failure(!string.IsNullOrWhiteSpace(error) ? error : "Failed to resend confirmation email.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resending confirmation email: {ex.Message}");
            return AuthResult.Failure("An error occurred while resending the confirmation email.");
        }
    }

    public async Task<bool> IsEmailConfirmedAsync(string email)
    {
        var response = await _httpClient.GetAsync($"api/auth/isemailconfirmed?email={Uri.EscapeDataString(email)}");
        if (!response.IsSuccessStatusCode) return false;

        return await response.Content.ReadFromJsonAsync<bool>();
    }

    

}