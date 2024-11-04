using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Http;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private bool _isInitialized;

    public AuthorizationMessageHandler(
        ILocalStorageService localStorage,
        IHttpContextAccessor httpContextAccessor)
    {
        _localStorage = localStorage;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Skip auth header for login/register endpoints
        if (request.RequestUri?.PathAndQuery.Contains("/api/auth/login") == true ||
            request.RequestUri?.PathAndQuery.Contains("/api/auth/register") == true)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        try
        {
            if (!_isInitialized)
            {
                // Skip token during prerender
                if (!_httpContextAccessor?.HttpContext?.Request.Headers.ContainsKey("User-Agent") ?? true)
                {
                    return await base.SendAsync(request, cancellationToken);
                }
                _isInitialized = true;
            }

            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch (InvalidOperationException)
        {
            // If we get a JS interop error, skip adding the auth header
            return await base.SendAsync(request, cancellationToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}