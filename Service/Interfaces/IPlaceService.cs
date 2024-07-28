using Service.DTOs.Places;

namespace Service.Interfaces
{
    public interface IPlaceService
    {
        Task CreateAsync(PlaceCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id, PlaceEditDto model);
        Task<PlaceDto> GetByIdAsync(int id);
        Task<IEnumerable<PlaceDto>> GetAllAsync();
    }
}
