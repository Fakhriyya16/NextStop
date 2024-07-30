
using Service.DTOs.Accounts;
using Service.Helpers;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<AccountManagementResponse> Register(RegisterDto model);
        Task<AccountManagementResponse> Login(LoginDto model);
        Task CreateRoles();
        Task<AccountManagementResponse> ConfirmEmail(string userId, string token);
        Task<AccountManagementResponse> ForgetPassword(string email);
        Task<AccountManagementResponse> ResetPassword(ResetPasswordDto model);
        Task<AccountManagementResponse> UpdateProfile(string userId,UserUpdateDto model);
        Task<AccountManagementResponse> DeleteProfile(string userId);
        Task<AccountManagementResponse> LogOut();
    }
}
