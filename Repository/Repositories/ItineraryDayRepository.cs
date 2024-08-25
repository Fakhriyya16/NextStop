using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;


namespace Repository.Repositories
{
    public class ItineraryDayRepository : BaseRepository<ItineraryDay>, IItineraryDayRepository
    {
        public ItineraryDayRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ItineraryDay>> GetAllByItineraryId(int itineraryId)
        {
            return await _entities.Where(m=>m.ItineraryId ==  itineraryId).Include(m=>m.ItineraryPlaces).ThenInclude(m=>m.Place).ThenInclude(m=>m.Category).ToListAsync();
        }
    }
}
