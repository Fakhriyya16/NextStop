
using Service.DTOs.Accounts;
using Service.Helpers;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<AccountManagementResponse> Register(RegisterDto model);
        Task<AccountManagementResponse> Login(LoginDto model);
    }
}
