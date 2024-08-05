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
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Payment> GetPaymentByStripeId(string stripeId)
        {
            return await _entities.FirstOrDefaultAsync(m => m.StripePaymentId == stripeId);
        }

        public async Task<Payment> GetPaymentByUserId(string userId)
        {
            return await _entities.FirstOrDefaultAsync(m => m.AppUserId == userId);
        }
    }
}
