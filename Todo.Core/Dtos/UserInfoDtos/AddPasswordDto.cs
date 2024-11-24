using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.UserInfoDtos;

public class AddPasswordDto
{
    [DataType(DataType.Password)]
    public string Password { get; set; } = String.Empty;
}