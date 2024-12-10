using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.UserInfoDtos;

public class DeleteAccountDto
{
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}