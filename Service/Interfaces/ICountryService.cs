using Service.DTOs.Countries;

namespace Service.Interfaces
{
    public interface ICountryService 
    {
        Task CreateAsync(CountryCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id,CountryEditDto model);
        Task<CountryDto> GetByIdAsync(int id);
        Task<IEnumerable<CountryDto>> GetAllAsync();
    }
}
