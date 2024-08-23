using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IPricingService
    {
        Task<SubscriptionPrice> GetCurrentPriceAsync();
        Task SetCurrentPriceAsync(decimal newPrice);
    }
}
