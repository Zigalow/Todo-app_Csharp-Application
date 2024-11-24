using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Todo.Api.Interfaces;
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

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
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

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        Console.WriteLine("Result " + result.Succeeded);
        if (!result.Succeeded)
        {
            Console.WriteLine(result.Errors.FirstOrDefault()?.Description);
            return BadRequest(result.Errors);
        }
        
       //await _userManager.AddToRoleAsync(user, "User");
        Console.WriteLine("User Created Token generate");
        var token = await GenerateJwtToken(user);

        return Ok(new { Token = token });
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

        // Add roles to claims
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