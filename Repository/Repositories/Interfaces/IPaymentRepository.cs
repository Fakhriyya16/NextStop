using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Repositories.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment> GetPaymentByUserId(string userId);
        Task<Payment> GetPaymentByStripeId(string stripeId);
        Task<IEnumerable<Payment>> SortBy(string property, string order);
    }
}
