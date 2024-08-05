
using Domain.Entities;
using Service.DTOs.Favorites;

namespace Service.Interfaces
{
    public interface IFavoriteService
    {
        Task CreateAsync(Favorite favorite);
        Task DeleteAsync(int? id);
        Task<Favorite> GetByIdAsync(int id);
        Task<IEnumerable<FavoriteDto>> GetAllAsync();
    }
}
