
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(AppDbContext context) : base(context)
        {

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
