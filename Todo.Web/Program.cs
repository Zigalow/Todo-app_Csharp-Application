using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Todo.Web.Auth;
using Todo.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "TodoApp.Auth";
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/logout"; // Provide a custom logout path
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });
builder.Services.AddAuthorization();

// Add custom auth state provider and auth service
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAntiforgery();

//Connect with todoApi
builder.Services
    .AddHttpClient("TodoApi", client => { client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!); })
    .AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAntiforgery();

app.Run();