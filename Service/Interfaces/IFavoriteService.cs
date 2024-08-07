
using Domain.Entities;
using Service.DTOs.Favorites;
using Service.Helpers;

namespace Service.Interfaces
{
    public interface IFavoriteService
    {
        Task CreateAsync(Favorite favorite);
        Task DeleteAsync(int? id);
        Task<Favorite> GetByIdAsync(int id);
        Task<IEnumerable<FavoriteDto>> GetAllAsync();
        Task<PaginateResponse<FavoriteDto>> GetAllPaginated(int currentPage, int pageSize);
    }
}
