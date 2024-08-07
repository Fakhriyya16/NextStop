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
    }
}
