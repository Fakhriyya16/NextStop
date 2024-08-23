
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Helpers;
using Repository.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Repository.Repositories
{
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<PaginationResponse<Favorite>> GetPaginationForUser(int currentPage, int pageSize,string userId)
        {
            var totalCount = await _entities.AsNoTracking().Where(m => m.AppUserId == userId).CountAsync();

            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var data = await _entities.AsNoTracking()
                                        .OrderBy(m => m.Id)
                                        .Include(m => m.AppUser)
                                        .Include(m => m.Place).ThenInclude(m => m.Images).Include(m => m.Place.City).ThenInclude(m => m.Country)
                                        .Where(m => m.AppUserId == userId)
                                        .Skip((currentPage - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

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

            var response = new PaginationResponse<Favorite>()
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

        public async Task<Favorite> IsFavorite(Expression<Func<Favorite, bool>> predicate)
        {
            return await _entities.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Favorite>> SortBy(string property, string order)
        {
            var data = await _entities.Include(m => m.AppUser).Include(m => m.Place).ToListAsync();

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
                default:
                    return data;
            }
        }
    }
}
