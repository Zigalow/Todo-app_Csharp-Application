using Todo.Web.Auth.Models;

namespace Todo.Web.Auth;

public interface IUserService
{
    Task<UserInfo?> GetUserInfoAsync();
    Task<bool> UpdatePhoneNumberAsync(string phoneNumber);
    Task<bool> HasExternalLoginsAsync();
}