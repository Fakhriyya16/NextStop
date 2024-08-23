
using Domain.Entities;
using Repository.Helpers;
using System.Linq.Expressions;

namespace Repository.Repositories.Interfaces
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Task<IEnumerable<Favorite>> SortBy(string property, string order);
        Task<Favorite> IsFavorite(Expression<Func<Favorite, bool>> predicate);
        Task<PaginationResponse<Favorite>> GetPaginationForUser(int currentPage, int pageSize, string userId);
    }
}
