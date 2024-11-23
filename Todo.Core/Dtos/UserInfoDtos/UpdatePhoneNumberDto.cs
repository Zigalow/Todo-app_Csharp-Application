using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.UserInfoDtos;

public class UpdatePhoneNumberDto
{
    public string PhoneNumber { get; set; } = string.Empty;
}