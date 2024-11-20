using Todo.Core.Dtos.AuthDto;
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
    Task<bool> ChangePasswordAsync(string oldPassword, string newPassword);
    Task<bool> AddPasswordAsync(string password);

    Task<bool> DeleteAccountAsync(string password);
    /*Two factor*/
    Task<TwoFactorInfo?> GetTwoFactorInfoAsync();
    Task ForgetTwoFactorClientAsync();
    Task<bool> Disable2FaAsync();
    Task<bool> Enable2FaAsync();
    Task<IEnumerable<string>> GenerateRecoveryCodesAsync();
    Task<bool> ResetAuthenticatorAsync();
    
}