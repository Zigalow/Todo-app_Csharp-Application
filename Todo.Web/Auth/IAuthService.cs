using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = Todo.Web.Auth.Models.LoginRequest;

namespace Todo.Web.Auth;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
}