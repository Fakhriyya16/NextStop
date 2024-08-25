using Repository.Helpers;
using Service.DTOs.Places;
using Service.Helpers;

namespace Service.Interfaces
{
    public interface IPlaceService
    {
        Task CreateAsync(PlaceCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id, PlaceEditDto model);
        Task<PlaceDto> GetByIdAsync(int id);
        Task<IEnumerable<PlaceDto>> GetAllAsync();
        Task<PaginateResponse<PlaceDto>> GetAllPaginated(int currentPage, int pageSize);
        Task<PaginateResponse<PlaceDto>> SortBy(string property, string order, int currentPage, int pageSize);
        Task<PaginateResponse<PlaceDto>> SearchByName(string searchText, int currentPage, int pageSize);
        Task<PaginateResponse<PlaceDto>> FilterByCategory(string category, int currentPage, int pageSize);
        Task<PaginateResponse<PlaceDto>> FilterByCity(string city, int currentPage, int pageSize);
        Task<PaginateResponse<PlaceDto>> FilterByTag(string tag, int currentPage, int pageSize);
    }
}
