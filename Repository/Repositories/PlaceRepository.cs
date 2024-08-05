using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
    {
        public PlaceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Place>> GetPlacesByCategoryAndCity(Category category, City city)
        {
            return await _entities.Where(m=>m.City == city).Where(m=>m.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Place>> GetPlacesByCityForItinerary(City city)
        {
            return await _entities.Where(m=>m.City == city).Include(m=>m.Category).ToListAsync();
        }

        public async Task<bool> IsExist(string name)
        {
            return await _entities.AnyAsync(e => e.Name == name);
        }
    }
}
