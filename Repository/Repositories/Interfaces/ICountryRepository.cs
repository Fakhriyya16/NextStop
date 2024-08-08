
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Task<Country> FindByName(string name);
        Task<IEnumerable<Country>> SearchByNameAsync(string searchText);
    }
}
