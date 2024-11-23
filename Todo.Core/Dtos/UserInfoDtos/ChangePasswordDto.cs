namespace Todo.Core.Dtos.AuthDto;

public class ChangePasswordDto
{
    
    public string OldPassword { get; set; } = String.Empty;
    public string NewPassword { get; set; } = String.Empty;
}