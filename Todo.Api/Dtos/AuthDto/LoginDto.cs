using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Dtos.AuthDto;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}