using Todo.Web.Auth.Models;

namespace Todo.Web.Auth;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginRequest request);
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<AuthResult> ResendConfirmationEmailAsync(string email);
    string? GetUserEmail();


}