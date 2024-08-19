using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Repository.Repositories.Interfaces;
using Service.Configurations;
using Service.DTOs.Payments;
using Service.Helpers.Exceptions;
using Service.Interfaces;
using Stripe;
using StripeSubscription = Stripe.Subscription;
using DomainSubscription = Domain.Entities.Subscription;

namespace Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IOptions<StripeSettings> _stripeSettings;
        private readonly IPaymentRepository _paymentRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISubscriptionService _subscriptionService;

        public PaymentService(IOptions<StripeSettings> stripeSettings,IPaymentRepository paymentRepository,
                              UserManager<AppUser> userManager, ISubscriptionService subscriptionService)
        {
            _stripeSettings = stripeSettings;
            _paymentRepository = paymentRepository;
            _userManager = userManager;
            _subscriptionService = subscriptionService;
        }

        public async Task CreateAsync(Payment payment)
        {
            await _paymentRepository.CreateAsync(payment);
        }

        public async Task<Payment> CreatePaymentAsync(PaymentIntentRequest request, string userId)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Description = request.Description,
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            var payment = new Payment
            {
                AppUserId = userId,
                StripePaymentId = paymentIntent.Id,
                Amount = request.Amount / 100m,  
                Currency = request.Currency,
                Status = paymentIntent.Status
            };

            await CreateAsync(payment);

            return payment;
        }

        public async Task ChangeSubscriptionType(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) throw new NotFoundException("User");

            Payment payment = await GetPaymentByUserId(user.Id);

            payment.Status = "Succeeded";
            
            await _paymentRepository.EditAsync(payment);

            var subscription = await _subscriptionService.GetByUserId(userId);

            if (subscription is null) throw new NotFoundException("Subscription");

            await _subscriptionService.DeleteAsync(subscription.Id);

            DomainSubscription newSubscription = new()
            {
                AppUserId = user.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                SubscriptionType = "Premium",
                IsActive = true,
            };

            user.Subscription = newSubscription;

            await _userManager.UpdateAsync(user);
        }

        public async Task<Payment> GetPaymentByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("User ID is required.");
            }

            var payment = await _paymentRepository.GetPaymentByUserId(userId);

            if (payment is null) throw new NotFoundException("Payment");

            return payment;
        }

        public async Task HandlePaymentCancellation(PaymentIntent paymentIntent)
        {
            var payment = await _paymentRepository.GetPaymentByStripeId(paymentIntent.Id);

            if (payment is not null)
            {
                payment.Status = "canceled";
                await _paymentRepository.EditAsync(payment);
            }
        }

        public async Task HandlePaymentFailure(PaymentIntent paymentIntent)
        {
            var payment = await _paymentRepository.GetPaymentByStripeId(paymentIntent.Id);

            if (payment is not null)
            {
                payment.Status = "failed";
                await _paymentRepository.EditAsync(payment);
            }
        }
    }
}
