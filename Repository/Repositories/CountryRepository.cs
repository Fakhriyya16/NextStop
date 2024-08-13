
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Country> FindByName(string name)
        {
            return await _entities.FirstOrDefaultAsync(m=>m.Name == name);
        }

        public async Task<IEnumerable<Country>> SearchByNameAsync(string searchText)
        {
            return await _entities.Where(m=>m.Name.Contains(searchText)).ToListAsync();
        }
    }
}
