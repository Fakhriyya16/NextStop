
using Domain.Entities;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        string GetToken(AppUser user,List<string> roles);
    }
}
