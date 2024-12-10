using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.UserInfoDtos;

public class ChangePasswordDto
{
    [Required]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; } = String.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = String.Empty;
}