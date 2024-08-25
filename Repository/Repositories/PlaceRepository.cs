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

            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var data = await _entities.AsNoTracking().OrderBy(m => m.Id).Include(m => m.Category)
                                      .Include(m => m.City).Include(m => m.Reviews).Include(m => m.Images)
                                      .Include(m => m.PlaceTags).ThenInclude(m=>m.Tag)
                                      .Skip((currentPage - 1) * pageSize)
                                      .Take(pageSize).ToListAsync();

            bool hasNext = currentPage < pageCount;
            bool hasPrevious = currentPage > 1;

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

        public async Task<PaginationResponse<Place>> SortBy(string property, string order, int currentPage, int pageSize)
        {
            IQueryable<Place> query = _entities.AsNoTracking()
                                      .Include(m => m.Category)
                                      .Include(m => m.City)
                                      .Include(m => m.Images)
                                      .Include(m => m.PlaceTags);

            switch (property.ToLower())
            {
                case "date":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(m => m.CreatedDate)
                        : query.OrderBy(m => m.CreatedDate);
                    break;

                case "rating":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(m => m.Rating)
                        : query.OrderBy(m => m.Rating);
                    break;

                default:
                    query = query.OrderBy(m => m.Id);
                    break;
            }

            var totalCount = await query.CountAsync();

            var data = await query.Skip((currentPage - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);
            bool hasNext = currentPage < pageCount;
            bool hasPrevious = currentPage > 1;

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

        public async Task<PaginationResponse<Place>> SearchByName(string searchText, int currentPage, int pageSize)
        {
            var query = _entities.AsNoTracking()
                                     .Where(m => m.Name.Contains(searchText))
                                     .Include(m => m.Category)
                                     .Include(m => m.City)
                                     .Include(m => m.Images)
                                     .Include(m => m.PlaceTags)
                                     .ThenInclude(pt => pt.Tag);

            var totalCount = await query.CountAsync();

            var data = await query.Skip((currentPage - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);
            bool hasNext = currentPage < pageCount;
            bool hasPrevious = currentPage > 1;

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

        public async Task<PaginationResponse<Place>> FilterByCategory(string category, int currentPage, int pageSize)
        {
            var totalCount = await _entities.AsNoTracking()
                                            .Where(m => m.Category.Name == category)
                                            .CountAsync();

            var data = await _entities.AsNoTracking()
                                      .Where(m => m.Category.Name == category)
                                      .OrderBy(m => m.Id)
                                      .Include(m => m.Category)
                                      .Include(m => m.City)
                                      .Include(m => m.Images)
                                      .Include(m => m.PlaceTags)
                                      .ThenInclude(m => m.Tag)
                                      .Skip((currentPage - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            bool hasNext = currentPage < pageCount;
            bool hasPrevious = currentPage > 1;

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

        public async Task<PaginationResponse<Place>> FilterByCity(string city, int currentPage, int pageSize)
        {
            var totalCount = await _entities.AsNoTracking()
                                            .Where(m => m.City.Name == city)
                                            .CountAsync();

            var data = await _entities.AsNoTracking()
                                      .Where(m => m.City.Name == city)
                                      .OrderBy(m => m.Id)
                                      .Include(m => m.Category)
                                      .Include(m => m.City)
                                      .Include(m => m.Images)
                                      .Include(m => m.PlaceTags)
                                      .ThenInclude(m => m.Tag)
                                      .Skip((currentPage - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);
            bool hasNext = currentPage < pageCount;
            bool hasPrevious = currentPage > 1;

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

        public async Task<PaginationResponse<Place>> FilterByTag(string tag, int currentPage, int pageSize)
        {
            var totalCount = await _entities.AsNoTracking()
                                                .Where(m => m.PlaceTags.Any(pt => pt.Tag.Name == tag))
                                                .CountAsync();

            var data = await _entities.AsNoTracking()
                                      .Where(m => m.PlaceTags.Any(pt => pt.Tag.Name == tag))
                                      .OrderBy(m => m.Id)
                                      .Include(m => m.Category)
                                      .Include(m => m.City)
                                      .Include(m => m.Images)
                                      .Include(m => m.PlaceTags)
                                      .ThenInclude(m => m.Tag)
                                      .Skip((currentPage - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);
            bool hasNext = currentPage < pageCount;
            bool hasPrevious = currentPage > 1;

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
    }
}
