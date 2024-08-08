
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IBlogRepository : IBaseRepository<Blog>
    {
        Task<IEnumerable<Blog>> SortBy(string property, string order);
        Task<bool> IsExistByTitle(string title);
        Task<IEnumerable<Blog>> SearchByTitle(string searchText);
    }
}
