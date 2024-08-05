
using Org.BouncyCastle.Bcpg;
using Service.DTOs.Reviews;

namespace Service.Interfaces
{
    public interface IReviewService
    {
        Task CreateAsync(ReviewCreateDto model, string userId, int placeId);
        Task DeleteAsync(int? id);
        Task<IEnumerable<ReviewDto>> GetAllForPlace(int? placeId);
        Task<IEnumerable<ReviewDto>> GetAllForUser(string userId);
        Task<ReviewDto> GetByIdAsync(int id);
    }
}
