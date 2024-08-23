using Domain.Entities;
using Repository.Helpers;

namespace Repository.Repositories.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<IEnumerable<Review>> GetAllForUser(string userId);
        Task<IEnumerable<Review>> GetAllForPlace(int placeId);
        Task<PaginationResponse<Review>> GetPaginationForPlace(int currentPage, int pageSize, int placeId);
    }
}
