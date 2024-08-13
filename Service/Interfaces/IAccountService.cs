using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Repository.Helpers;
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
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDetailDto> GetUserById(string id);
        Task<AccountManagementResponse> AddRoleToUser(string userId, string role);
        Task<AccountManagementResponse> RemoveRoleFromUser(string userId, string role);
        Task<PaginationResponse<UserDto>> GetPaginatedUsers(int currentPage, int pageSize);
        bool IsAuthenticated(HttpContext httpContext);
        Task<AppUser> GetAuthenticatedUser(HttpContext httpContext);
    }
}
