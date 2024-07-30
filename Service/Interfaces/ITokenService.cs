
using Domain.Entities;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        string GetToken(AppUser user,IList<string> roles);
    }
}
