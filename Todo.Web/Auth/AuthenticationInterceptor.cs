using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Todo.Web.Auth;

public class AuthenticationInterceptor : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        // Exclude login and registration endpoints
        if (request.RequestUri != null &&
            (request.RequestUri.AbsolutePath.Contains("/api/auth/login") ||
             request.RequestUri.AbsolutePath.Contains("/api/auth/register")))
        {
            return response;
        }

        // Check if the response is an unauthorized response (401)
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Clear authentication
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Set a flag in TempData to trigger client-side redirect
                httpContext.Response.Cookies.Append("auth_redirect", "true", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(1)
                });
            }
        }

        return response;
    }
}