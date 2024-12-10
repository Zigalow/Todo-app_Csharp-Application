using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Dtos.AuthDto;
using Todo.Core.Dtos.UserInfoDtos;
using Todo.Core.Entities;

namespace Todo.Api.Controllers;
[Authorize]
[Route("api/user")]
[ApiController]
public class UserController: ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    
    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpGet("userinfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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
    
    [HttpGet("userIdFromEmail")]
    public async Task<IActionResult> GetUserIdFromEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }

        var user = await _userManager.FindByEmailAsync(email);
    
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user.Id);
    }
    
    [HttpGet("userEmailFromName")]
    public async Task<IActionResult> GetUserEmailFromName (string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name is required.");
        }

        var user = await _userManager.FindByNameAsync(name);
    
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user.Email);
    }
    
    [HttpPost("updatePhoneNumber")]
    public async Task<IActionResult> UpdatePhoneNumberAsync(UpdatePhoneNumberDto phoneNumberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        Console.WriteLine("User phone: " + phoneNumberDto.PhoneNumber);
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
        var result = await _userManager.SetPhoneNumberAsync(user, phoneNumberDto.PhoneNumber);
        
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
    public async Task<IActionResult> UpdateEmailAsync(UpdateEmailDto updateEmailDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        Console.WriteLine("New email: " + updateEmailDto.Email);
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
        var existingUser = await _userManager.FindByEmailAsync(updateEmailDto.Email);
        if (existingUser != null && existingUser.Id != userId)
        {
            return BadRequest("Email is already in use.");
        }
        
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, updateEmailDto.Email);
        var result = await _userManager.ChangeEmailAsync(user, updateEmailDto.Email, token);
    
        Console.WriteLine("Update Email Result: " + result.Succeeded);
        if (!result.Succeeded)
        {
            return BadRequest("Failed to update email.");
        }

        Console.WriteLine("Updated Email");
        return Ok("Email updated successfully.");
    }

    [HttpGet("hasPassword")]
    public async Task<IActionResult> HasPasswordAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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

        var hasPassword = await _userManager.HasPasswordAsync(user);
        Console.WriteLine("Password status: "+ hasPassword);
        
        return Ok(hasPassword);
    }

    [HttpGet("isEmailConfirmed")]
    public async Task<IActionResult> IsEmailConformedAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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

    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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
        
        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        await _signInManager.RefreshSignInAsync(user);
        return Ok(changePasswordDto);
    }
    
    [HttpGet("setPassword")]
    public async Task<IActionResult> SetPasswordAsync(AddPasswordDto addPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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
        
        var result = await _userManager.AddPasswordAsync(user, addPasswordDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        await _signInManager.RefreshSignInAsync(user);
        return Ok("Password set successfully.");
    }
    [HttpPost("deleteAccount")]
    public async Task<IActionResult> DeleteAccountAsync(DeleteAccountDto deleteAccountDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        
        if (!string.IsNullOrEmpty(deleteAccountDto.Password))
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, deleteAccountDto.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Incorrect password.");
            }
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest("Failed to delete account.");
        }

        await _signInManager.SignOutAsync();
        return Ok();
    }
    
    /*-----BEGIN: TwoFactor-----*/
    [HttpGet("twoFactorInfo")]
    public async Task<IActionResult> GetTwoFactorInfo()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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
        
        var key = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(key))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            key = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        var information = new TwoFactorInfoDto
        {
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) is not null,
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            SharedKey = key
        };
        
        Console.WriteLine("Found User information");
        return Ok(information);
    }
    
    [HttpPost("forgetTwoFactorClient")]
    public async Task<IActionResult> ForgetTwoFactorClientAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        await _signInManager.ForgetTwoFactorClientAsync();
        return Ok();
    }
    
    [HttpPost("disableTwoFactor")]
    public async Task<IActionResult> Disable2FaAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disable2faResult.Succeeded)
        {
            return BadRequest("Failed to disable two-factor authentication");
        }

        return Ok();
    }
    [HttpPost("enableTwoFactor")]
    public async Task<IActionResult> Enable2FaAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var enable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, true);
        if (!enable2faResult.Succeeded)
        {
            return BadRequest("Failed to disable two-factor authentication");
        }

        return Ok();
    }

    [HttpPost("generateRecoveryCodes")]
    public async Task<IActionResult> GenerateRecoveryCodesAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
    
        return Ok(recoveryCodes);
    }

    [HttpPost("resetAuthenticator")]
    public async Task<IActionResult> ResetAuthenticatorAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        // Disable twoFactor authentication
        await _userManager.SetTwoFactorEnabledAsync(user, false);
    
        // Reset authenticator key
        await _userManager.ResetAuthenticatorKeyAsync(user);
    
        return Ok();
    }
    
    /*-----END: TwoFactor-----*/
    
    
}