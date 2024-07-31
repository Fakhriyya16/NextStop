
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IBlogRepository : IBaseRepository<Blog>
    {
        Task<bool> IsExistByTitle(string title); 
    }
}
