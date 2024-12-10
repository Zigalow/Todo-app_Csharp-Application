using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Todo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected string GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User is not authenticated");
        return userIdClaim.Value;
    }

    protected string GetCurrentUserEmail()
    {
        var emailClaim = User.FindFirst(ClaimTypes.Email);
        if (emailClaim == null)
            throw new UnauthorizedAccessException("User is not authenticated");
        return emailClaim.Value;
    }

    protected bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;
}