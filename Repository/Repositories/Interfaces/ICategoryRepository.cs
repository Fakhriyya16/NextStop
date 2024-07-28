
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> IsExist(string name);
        Task<bool> IsExistById(int id);
    }
}
