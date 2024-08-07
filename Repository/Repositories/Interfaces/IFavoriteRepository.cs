
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Task<IEnumerable<Favorite>> SortBy(string property, string order);
    }
}
