
using Domain.Entities;
using Repository.Data;
using Repository.Repositories;
using Repository.Repositories.Interfaces;

namespace Repository
{
    public class ItineraryPlaceRepository : BaseRepository<ItineraryPlace>, IItineraryPlaceRepository
    {
        public ItineraryPlaceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
