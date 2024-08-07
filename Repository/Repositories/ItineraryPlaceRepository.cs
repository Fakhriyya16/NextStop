
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class ItineraryPlaceRepository : BaseRepository<ItineraryPlace>, IItineraryPlaceRepository
    {
        public ItineraryPlaceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
