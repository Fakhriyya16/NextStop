
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    internal class PricingRepository : BaseRepository<SubscriptionPrice>, IPricingRepository
    {
        public PricingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<SubscriptionPrice> GetCurrentPriceAsync()
        {
            return await _entities.FirstOrDefaultAsync();
        }
    }
}
