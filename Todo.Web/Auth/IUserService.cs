using Todo.Core.Dtos.AuthDto;
using Todo.Core.Dtos.UserInfoDtos;
using Todo.Web.Auth.Models;

namespace Todo.Web.Auth;

public interface IUserService
{
    Task<UserInfoResponse?> GetUserInfoAsync();
    Task<string?> GetUserIdFromEmailAsync(string email);
    Task<string?> GetUserEmailFromNameAsync(string name);
    Task<bool> UpdatePhoneNumberAsync(string phoneNumber);
    Task<bool> HasExternalLoginsAsync();
    Task<bool> UpdateEmailAsync(UpdateEmailDto info);
    Task<bool> IsEmailConfirmedAsync();
    Task<bool> HasPasswordAsync();
    Task<bool> ChangePasswordAsync(string oldPassword, string newPassword);
    Task<bool> AddPasswordAsync(string password);

    Task<bool> DeleteAccountAsync(string password);
    /*Two factor*/
    Task<TwoFactorInfoDto?> GetTwoFactorInfoAsync();
    Task ForgetTwoFactorClientAsync();
    Task<bool> Disable2FaAsync();
    Task<bool> Enable2FaAsync();
    Task<IEnumerable<string>> GenerateRecoveryCodesAsync();
    Task<bool> ResetAuthenticatorAsync();
    
}