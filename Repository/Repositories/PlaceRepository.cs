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

        public async Task<bool> IsExist(string name)
        {
            return await _entities.AnyAsync(e => e.Name == name);
        }
    }
}
