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

        public async Task<IEnumerable<Payment>> SortBy(string property, string order)
        {
            var data = await _entities.Include(m => m.AppUser).ToListAsync();

            switch (property)
            {
                case "date":
                    if (order == "desc")
                    {
                        return data.OrderByDescending(m => m.CreatedDate);
                    }
                    else if (order == "asc")
                    {
                        return data.OrderBy(m => m.CreatedDate);
                    }
                    else
                    {
                        return data;
                    }
                case "amount":
                    if (order == "desc")
                    {
                        return data.OrderByDescending(m => m.Amount);
                    }
                    else if (order == "asc")
                    {
                        return data.OrderBy(m => m.Amount);
                    }
                    else
                    {
                        return data;
                    }
                default:
                    return data;
            }
        }
    }
}
