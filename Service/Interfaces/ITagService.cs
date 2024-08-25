using Service.DTOs.Tags;

namespace Service.Interfaces
{
    public interface ITagService
    {
        Task CreateAsync(TagCreateDto model);
        Task DeleteAsync(int? id);
        Task EditAsync(int? id, TagEditDto model);
        Task<TagDto> GetByIdAsync(int id);
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<IEnumerable<TagNameDto>> GetAllNames();
    }
}
