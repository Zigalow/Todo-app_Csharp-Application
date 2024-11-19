namespace Todo.Core.Dtos.AuthDto;

public class PasswordDto
{
    
    public string? OldPassword { get; set; } = String.Empty;
    public string? NewPassword { get; set; } = String.Empty;
}