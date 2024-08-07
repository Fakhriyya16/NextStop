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

        public async Task<IEnumerable<Review>> SortBy(string property, string order)
        {
            var data = await _entities.Include(m => m.Place)
                                      .Include(m => m.AppUser)
                                      .ToListAsync();

            switch (property)
            {
                case "date":
                    if (order == "desc")
                    {
                        return data.OrderByDescending(m => m.CreatedDate);
                    }
                    else if (order == "asc")
                    {
                        return data.OrderBy(m => m.CreatedDate);
                    }
                    else
                    {
                        return data;
                    }
                case "rating":
                    if (order == "desc")
                    {
                        return data.OrderByDescending(m => m.Rating);
                    }
                    else if (order == "asc")
                    {
                        return data.OrderBy(m => m.Rating);
                    }
                    else
                    {
                        return data;
                    }
                default:
                    return data;
            }
        }
    }
}
