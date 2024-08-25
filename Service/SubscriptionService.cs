using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task CreateAsync(Subscription model)
        {
            await _subscriptionRepository.CreateAsync(model);
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var subscription = await _subscriptionRepository.GetById((int)id);

            if (subscription is null) throw new NotFoundException("Subscription");

            await _subscriptionRepository.DeleteAsync(subscription);
        }

        public async Task DeleteFromDatabase(Subscription subscription)
        {
            if (subscription is null) throw new NotFoundException("Subscription");

            await _subscriptionRepository.DeleteFromDatabase(subscription);
        }

        public async Task EditAsync(int? id, Subscription model)
        {
            if (id is null) throw new ArgumentNullException();

            var subscription = await _subscriptionRepository.GetById((int)id);

            if (subscription is null) throw new NotFoundException("Subscription");

            await _subscriptionRepository.EditAsync(subscription);
        }

        public async Task<Subscription> GetByUserId(string userId)
        {
            return await _subscriptionRepository.GetByUserId(userId);
        }
    }
}
