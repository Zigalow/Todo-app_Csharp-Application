using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Todo.Api.Interfaces;
using Todo.Api.Services;
using Todo.Core.Dtos.AuthDto;
using Todo.Core.Entities;

namespace Todo.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmailService _emailService;
    

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration, IUnitOfWork unitOfWork, EmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    [HttpGet("isemailconfirmed")]
    public async Task<IActionResult> IsEmailConfirmed(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound("User not found.");

        var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        return Ok(isConfirmed);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        Console.WriteLine("Register");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Console.WriteLine("Create new user");
        var user = new ApplicationUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email
        };

        var existingEmail = _userManager.FindByEmailAsync(registerDto.Email);

        if (existingEmail.Result != null)
        {
            return BadRequest("User with email already exists");
        }

        var existingUsername = _userManager.FindByNameAsync(registerDto.UserName);

        if (existingUsername.Result != null)
        {
            return BadRequest("User with username already exists");
        }

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        Console.WriteLine("Result " + result.Succeeded);

        if (!result.Succeeded)
        {
            Console.WriteLine(result.Errors.FirstOrDefault()?.Description);
            return BadRequest(result.Errors);
        }
        
        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(
            "ConfirmEmail",
            "Auth",
            new { userId = user.Id, token = emailConfirmationToken },
            Request.Scheme
        );

        var emailContent = $@"
            <h1>Welcome to Taskify!</h1>
            <p>Please confirm your registration by clicking the link below:</p>
            <a href='{confirmationLink}'>Confirm your email</a>";

        await SendEmailWithExceptionHandling(user.Email, emailContent);

        var token = await GenerateJwtToken(user);
        return Ok(new { Token = token, Message = "Registration successful! A confirmation email has been sent." });

    }

    [HttpPost("resend-confirmation-email")]
public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
{
    var email = request.Email;
    var user = await _userManager.FindByEmailAsync(email);
    
    if (user == null)
    {
        return BadRequest("No user found with the provided email.");
    }

    if (await _userManager.IsEmailConfirmedAsync(user))
    {
        return BadRequest("This email has already been confirmed.");
    }

    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(
            "ConfirmEmail",
            "Auth",
            new { userId = user.Id, token = emailConfirmationToken },
            Request.Scheme
        );

        var emailContent = $@"
            <h1>Welcome to Taskify!</h1>
            <p>You have requested to resend the confirmation e-mail</p>
            <p>Please confirm your registration by clicking the link below:</p>
            <a href='{confirmationLink}'>Confirm your email</a>";

        await SendEmailWithExceptionHandling(user.Email, emailContent);

        return Ok("A new confirmation email has been sent.");
}
private async Task SendEmailWithExceptionHandling(string recipientEmail, string emailContent)
    {
        try
        {
            await _emailService.SendConfirmationEmail(recipientEmail, emailContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email to {recipientEmail}: {ex.Message}");
            throw new InvalidOperationException("An error occurred while sending the email.", ex);
        }
    }
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest("Invalid user ID");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return Ok("Email confirmed successfully!");
        }

        return BadRequest("Email confirmation failed.");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        Console.WriteLine("Login begin");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
        {
            Console.WriteLine("User is null");
            return BadRequest("Invalid credentials");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            Console.WriteLine("Result not success");
            return BadRequest("Invalid credentials");
        }

        var token = await GenerateJwtToken(user);
        Console.WriteLine("Login end");
        return Ok(new { Token = token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { Message = "Logged out successfully" });
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(
            Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}