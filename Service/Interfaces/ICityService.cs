
using Service.DTOs.Cities;
using Service.DTOs.Countries;

namespace Service.Interfaces
{
    public interface ICityService
    {
        Task CreateAsync(CityCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id, CityEditDto model);
        Task<CityDto> GetByIdAsync(int id);
        Task<IEnumerable<CityDto>> GetAllAsync();
        Task<IEnumerable<CityNameDto>> GetAllNamesAsync();
        Task<CityDto> GetByName(string city);
    }
}
