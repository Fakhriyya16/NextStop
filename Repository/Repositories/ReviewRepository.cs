using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetAllForPlace(int placeId)
        {
            return await _entities.Where(m=>m.PlaceId == placeId).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllForUser(string userId)
        {
            return await _entities.Where(m => m.AppUserId == userId).ToListAsync();
        }
    }
}
