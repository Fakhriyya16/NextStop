
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class BlogRepository : BaseRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsExistByTitle(string title)
        {
            return await _entities.AnyAsync(e => e.Title == title);
        }

        public async Task<IEnumerable<Blog>> SortBy(string property,string order)
        {
             var data = await _entities.Include(m => m.AppUser).Include(m => m.BlogImages).ToListAsync();

            switch (property)
            {
                case "date":
                    if(order == "desc")
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
