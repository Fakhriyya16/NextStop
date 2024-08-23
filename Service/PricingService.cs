using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Interfaces;


namespace Service
{
    public class PricingService : IPricingService
    {
        private readonly IPricingRepository _pricingRepository;

        public PricingService(IPricingRepository pricingRepository)
        {
            _pricingRepository = pricingRepository;
        }

        public async Task<SubscriptionPrice> GetCurrentPriceAsync()
        {
            return await _pricingRepository.GetCurrentPriceAsync();
        }

        public async Task SetCurrentPriceAsync(decimal newPrice)
        {
            var currentPrice = await _pricingRepository.GetCurrentPriceAsync();
            if (currentPrice == null)
            {
                currentPrice = new SubscriptionPrice { MonthlyPrice = newPrice };
                await _pricingRepository.CreateAsync(currentPrice);
            }
            else
            {
                currentPrice.MonthlyPrice = newPrice;
                await _pricingRepository.EditAsync(currentPrice);
            }
        }
    }
}
