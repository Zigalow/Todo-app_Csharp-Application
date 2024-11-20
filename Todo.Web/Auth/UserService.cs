using Todo.Core.Dtos.AuthDto;
using Todo.Web.Auth.Models;

namespace Todo.Web.Auth;

public class UserService: IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserService(IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("TodoApi");
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<UserInfoRequest?> GetUserInfoAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/user/userinfo");

            if (!response.IsSuccessStatusCode) return null;
            
            return await response.Content.ReadFromJsonAsync<UserInfoRequest>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user info: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> UpdatePhoneNumberAsync(string phoneNumber)
    {
        try
        {
            var info = new UserInfoRequest
            {
                PhoneNumber = phoneNumber
            };

            var response = await _httpClient.PostAsJsonAsync("api/user/updatePhoneNumber",  info);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating phone number: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> HasExternalLoginsAsync()
    {
        /*TODO:Implement logic*/
        return false;
    }

    public async Task<bool> IsEmailConfirmedAsync()
    {
        try
        {

            var response = await _httpClient.GetAsync("api/user/isEmailConfirmed");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating phone number: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateEmailAsync(UserInfoRequest info)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/updateEmail",  info);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Error Content: {content}");
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating email: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> HasPasswordAsync()
    {
        try
        {

            var response = await _httpClient.GetAsync("api/user/hasPassword");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating phone number: {ex.Message}");
            return false;
        }
            
    }

    public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
    {
        try
        {
            var newPasswordRequest = new ChangePasswordRequest()
            {
                NewPassword = newPassword,
                OldPassword = oldPassword
            };
            var response = await _httpClient.PostAsJsonAsync("api/user/changePassword",newPasswordRequest);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Error Content: {content}");
                return   false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating password: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> AddPasswordAsync(string password)
    {
        try
        {
            var newPasswordRequest = new ChangePasswordRequest()
            {
                NewPassword = password,
            };
            var response = await _httpClient.PostAsJsonAsync("api/user/setPassword", newPasswordRequest);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting password: {ex.Message}");
            return false;
        }   
    }
    
    public async Task<bool> DeleteAccountAsync(string password)
    {
        try
        {
            var request = new PasswordDto 
            { 
                OldPassword = password
            };
        
            var response = await _httpClient.PostAsJsonAsync("api/user/deleteAccount", request);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Error Content: {content}");
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting account: {ex.Message}");
            return false;
        }
    }
    
    public async Task<TwoFactorInfo?> GetTwoFactorInfoAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/user/twoFactorInfo");
            if (!response.IsSuccessStatusCode) return null;
            
            return await response.Content.ReadFromJsonAsync<TwoFactorInfo>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Two Factor info: {ex.Message}");
            return null;
        }      
    }
    
    public async Task ForgetTwoFactorClientAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/user/forgetTwoFactorClient", null);
        
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Forget Two-Factor Client Failed. Status: {response.StatusCode}, Error: {errorContent}");
                throw new Exception("Failed to forget two-factor client");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error forgetting two-factor client: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> Disable2FaAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/user/disableTwoFactor", null);
            return response.IsSuccessStatusCode;
           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error disabling two-factor authentication: {ex.Message}");
            throw;
        }

    }

    public async Task<bool> Enable2FaAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/user/EnableTwoFactor", null);
            return response.IsSuccessStatusCode;
           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error disabling two-factor authentication: {ex.Message}");
            throw;
        }

    }
    public async Task<IEnumerable<string>> GenerateRecoveryCodesAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/user/generateRecoveryCodes", null);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Generate Recovery Codes Failed. Status: {response.StatusCode}, Error: {errorContent}");
                throw new Exception("Failed to generate recovery codes");
            }

            var recoveryCodes = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            return recoveryCodes ?? Enumerable.Empty<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating recovery codes: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> ResetAuthenticatorAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/user/resetAuthenticator", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resetting authenticator: {ex.Message}");
            throw;
        }
    }
}