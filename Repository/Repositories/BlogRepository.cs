using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Helpers;
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

        public async Task<IEnumerable<Blog>> SearchByTitle(string searchText)
        {
            return await _entities.Where(m => m.Title.Contains(searchText)).ToListAsync();
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

        public async override Task<PaginationResponse<Blog>> GetPagination(int currentPage, int pageSize)
        {
            var totalCount = await _entities.AsNoTracking().CountAsync();

            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var data = await _entities.AsNoTracking().OrderBy(m => m.Id).Include(m => m.AppUser)
                                      .Include(m => m.BlogImages)
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

            var response = new PaginationResponse<Blog>()
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
