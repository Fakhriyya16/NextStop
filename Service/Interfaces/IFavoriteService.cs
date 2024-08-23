
using Domain.Entities;
using Service.DTOs.Favorites;
using Service.Helpers;

namespace Service.Interfaces
{
    public interface IFavoriteService
    {
        Task CreateAsync(FavoriteCreateDto favorite);
        Task DeleteAsync(string userId, int placeId);
        Task<Favorite> GetByIdAsync(int id);
        Task<IEnumerable<FavoriteDto>> GetAllAsync();
        Task<PaginateResponse<FavoriteDto>> GetAllPaginated(int currentPage, int pageSize);
        Task<bool> IsFavoriteAsync(string userId, int placeId);
        Task<PaginateResponse<FavoriteDto>> GetAllPaginatedForUser(int currentPage, int pageSize, string userId);
    }
}
