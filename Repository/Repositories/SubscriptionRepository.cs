using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task DeleteFromDatabase(Subscription subscription)
        {
             _entities.Remove(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task<Subscription> GetByUserId(string userId)
        {
            return await _entities.FirstOrDefaultAsync(m => m.AppUserId == userId);
        }
    }
}
