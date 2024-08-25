using Domain.Entities;
using Repository.Helpers;

namespace Repository.Repositories.Interfaces
{
    public interface IPlaceRepository : IBaseRepository<Place>
    {
        Task<bool> IsExist(string name);
        Task<IEnumerable<Place>> GetPlacesByCategoryAndCity(Category category,City city);
        Task<IEnumerable<Place>> GetPlacesByCityForItinerary(City city);
        Task<PaginationResponse<Place>> SortBy(string property, string order, int currentPage, int pageSize);
        Task<PaginationResponse<Place>> SearchByName(string searchText, int currentPage, int pageSize);
        Task<PaginationResponse<Place>> FilterByCategory(string category, int currentPage, int pageSize);
        Task<PaginationResponse<Place>> FilterByCity(string city, int currentPage, int pageSize);
        Task<PaginationResponse<Place>> FilterByTag(string tag, int currentPage, int pageSize);
    }
}
