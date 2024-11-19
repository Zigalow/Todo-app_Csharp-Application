using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Dtos.AuthDto;
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
    
        var userInfo = new ApplicationUser()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
        };
        
        Console.WriteLine("Found User information");
        return Ok(userInfo);
    }
    
    [HttpPost("updatePhoneNumber")]
    public async Task<IActionResult> UpdatePhoneNumberAsync(ApplicationUser userInfo)
    {
        Console.WriteLine("User phone: " + userInfo.PhoneNumber);
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
    
    [HttpPost("updateEmail")]
    public async Task<IActionResult> UpdateEmailAsync(UserInfoDto userInfo)
    {
        Console.WriteLine("New email: " + userInfo.Email);
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

        // Check if new email is already being used
        var existingUser = await _userManager.FindByEmailAsync(userInfo.Email);
        if (existingUser != null && existingUser.Id != userId)
        {
            return BadRequest("Email is already in use.");
        }
        
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, userInfo.Email);
        var result = await _userManager.ChangeEmailAsync(user, userInfo.Email, token);
    
        Console.WriteLine("Update Email Result: " + result.Succeeded);
        if (!result.Succeeded)
        {
            return BadRequest("Failed to update email.");
        }

        Console.WriteLine("Updated Email");
        return Ok("Email updated successfully.");
    }

    [HttpGet("isEmailConfirmed")]
    public async Task<IActionResult> IsEmailConformedAsync()
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

        var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        Console.WriteLine("Email confirmed status: "+ isConfirmed);
        
        return Ok(isConfirmed);
    }
}