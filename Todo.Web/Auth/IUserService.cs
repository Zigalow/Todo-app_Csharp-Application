using Todo.Web.Auth.Models;

namespace Todo.Web.Auth;

public interface IUserService
{
    Task<UserInfoRequest?> GetUserInfoAsync();
    Task<bool> UpdatePhoneNumberAsync(string phoneNumber);
    Task<bool> HasExternalLoginsAsync();
    Task<bool> UpdateEmailAsync(UserInfoRequest info);
    Task<bool> IsEmailConfirmedAsync();
    Task<bool> HasPasswordAsync();
}