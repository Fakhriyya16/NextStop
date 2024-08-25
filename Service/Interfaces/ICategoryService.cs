
using Service.DTOs.Categories;
using Service.DTOs.Countries;

namespace Service.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id, CategoryEditDto model);
        Task<CategoryDto> GetByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<IEnumerable<CategoryNameDto>> GetAllNamesAsync();
    }
}
