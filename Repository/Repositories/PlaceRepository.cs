using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Helpers;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
    {
        public PlaceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Place>> GetPlacesByCategoryAndCity(Category category, City city)
        {
            return await _entities.Where(m => m.City == city).Where(m => m.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Place>> GetPlacesByCityForItinerary(City city)
        {
            return await _entities.Where(m => m.City == city).Include(m => m.Category).ToListAsync();
        }

        public async Task<bool> IsExist(string name)
        {
            return await _entities.AnyAsync(e => e.Name == name);
        }

        public async override Task<PaginationResponse<Place>> GetPagination(int currentPage, int pageSize)
        {
            var totalCount = await _entities.AsNoTracking().CountAsync();

            int pageCount = (int)Math.Ceiling((double)(totalCount / pageSize));

            var data = await _entities.AsNoTracking().OrderBy(m => m.Id).Include(m => m.Category)
                                      .Include(m => m.City).Include(m => m.Reviews).Include(m => m.PlaceTags)
                                      .Include(m => m.Images)
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

            var response = new PaginationResponse<Place>()
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

        public async Task<IEnumerable<Place>> SortBy(string property, string order)
        {
            var data = await _entities.Include(m => m.Category).Include(m => m.City)
                                      .Include(m => m.Images).Include(m => m.PlaceTags)
                                      .Include(m => m.Reviews).ToListAsync();

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

        public async Task<IEnumerable<Place>> SearchByName(string searchText)
        {
            return await _entities.Where(m => m.Name.Contains(searchText)).ToListAsync();
        }

        public async Task<IEnumerable<Place>> FilterByCategory(string category)
        {
            return await _entities.Where(m => m.Category.Name == category).ToListAsync();
        }

        public async Task<IEnumerable<Place>> FilterByCity(string city)
        {
            return await _entities.Where(m => m.City.Name == city).ToListAsync();
        }

        public async Task<IEnumerable<Place>> FilterByTag(string tag)
        {
            return await _entities.Where(m => m.PlaceTags.Any(pt => pt.Tag.Name == tag))
                                  .ToListAsync();
        }
    }
}
