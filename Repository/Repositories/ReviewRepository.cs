using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Helpers;
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
            return await _entities.Include(m=>m.AppUser).Where(m=>m.PlaceId == placeId).ToListAsync();
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

        public async Task<PaginationResponse<Review>> GetPaginationForPlace(int currentPage, int pageSize,int placeId)
        {
            var totalCount = await _entities.AsNoTracking().CountAsync();

            int pageCount = (int)Math.Ceiling((double)(totalCount / pageSize));

            var data = await _entities.AsNoTracking().OrderBy(m => m.Id).Include(m => m.AppUser).Where(m=>m.PlaceId == placeId)
                                      .Skip((currentPage - 1) * pageSize)
                                      .Take(pageSize).ToListAsync();

            bool hasNext = true;
            bool hasPrevious = true;

            if (currentPage == 1)
            {
                hasPrevious = false;
            }
            if (currentPage == pageCount)
            {
                hasNext = false;
            }

            var response = new PaginationResponse<Review>()
            {
                Data = data,
                TotalCount = totalCount,
                CurrentPage = currentPage,
                PageCount = pageCount,
                PageSize = pageSize,
                HasNext = hasNext,
                HasPrevious = hasPrevious,
            };

            return response;
        }
    }
}
