using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.UserInfoDtos;

public class UpdateEmailDto
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}