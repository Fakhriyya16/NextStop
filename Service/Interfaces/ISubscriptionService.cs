
using Domain.Entities;

namespace Service.Interfaces
{
    public interface ISubscriptionService
    {
        Task CreateAsync(Subscription model);
        Task EditAsync(int? id,Subscription model);
        Task DeleteAsync(int? id);
        Task<Subscription> GetByUserId(string userId);
    }
}
