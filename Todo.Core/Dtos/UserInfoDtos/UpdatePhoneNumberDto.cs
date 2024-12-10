using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dtos.UserInfoDtos;

public class UpdatePhoneNumberDto
{
    [Phone]
    //int values with minimum 6 digits and max 12
    [RegularExpression(@"(^\+\d{10})|(^\d{6,12})$")]
    public string PhoneNumber { get; set; } = string.Empty;
}