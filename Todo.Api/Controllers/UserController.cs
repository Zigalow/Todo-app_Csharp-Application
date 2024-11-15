using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Entities;

namespace Todo.Api.Controllers;

[Route("api/user")]
[ApiController]
public class UserController: ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpGet("userinfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            Console.WriteLine("User is null");
            return Unauthorized();
        }
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }
    
        var userInfo = new ApplicationUser
        {
            UserName = user.UserName,
            Email = user.Email,
        };
        
        Console.WriteLine("Found User information");
        return Ok(userInfo);
    }
    
    [HttpPost("updatePhoneNumber")]
    public async Task<IActionResult> UpdatePhoneNumberAsync(ApplicationUser userInfo)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
        {
            Console.WriteLine("UserId not found");
            return Unauthorized();
        }
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            Console.WriteLine("User is null");
            return NotFound("User not found.");
        }
        
        //Set users phone number
        var result = await _userManager.SetPhoneNumberAsync(user, userInfo.PhoneNumber);
        
        Console.WriteLine("Update PhoneNumber Result: " + result.Succeeded);
        if (!result.Succeeded)
        {
            Console.WriteLine(result.Errors.FirstOrDefault()?.Description);
            return BadRequest("Failed to update phone number.");
        }
        
        Console.WriteLine("Phone Number updated");
        return Ok("Phone number updated and confirmed successfully.");
    }
}