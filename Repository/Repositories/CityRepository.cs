
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<string> GetCountryNameByCityId(int cityId)
        {
            var city = await _entities.FirstOrDefaultAsync(m=>m.Id == cityId);

            return city.Country.Name;
        }

        public async Task<bool> IsExist(string name)
        {
            return await _entities.AnyAsync(e => e.Name == name);
        }
    }
}
