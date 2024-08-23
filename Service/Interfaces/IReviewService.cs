
using Org.BouncyCastle.Bcpg;
using Service.DTOs.Reviews;
using Service.Helpers;

namespace Service.Interfaces
{
    public interface IReviewService
    {
        Task CreateAsync(ReviewCreateDto model, string userId, int placeId);
        Task DeleteAsync(int? id);
        Task<IEnumerable<ReviewDto>> GetAllForPlace(int? placeId);
        Task<IEnumerable<ReviewDto>> GetAllForUser(string userId);
        Task<ReviewDto> GetByIdAsync(int id);
        Task<PaginateResponse<ReviewDto>> GetAllPaginated(int currentPage, int pageSize,int placeId);
    }
}
