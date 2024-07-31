
using Service.DTOs.Blogs;

namespace Service.Interfaces
{
    public interface IBlogService
    {
        Task CreateAsync(BlogCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id, BlogEditDto model);
        Task<IEnumerable<BlogDto>> GetAllAsync();
        Task<BlogDto> GetByIdAsync(int id);
    }
}
