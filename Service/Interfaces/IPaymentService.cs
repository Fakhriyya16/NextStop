
using Domain.Entities;
using Service.DTOs.Payments;
using Stripe;

namespace Service.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreatePaymentAsync(PaymentIntentRequest request, string userId);
        Task CreateAsync(Payment payment);
        Task ChangeSubscriptionType(string userId);
        Task<Payment> GetPaymentByUserId(string userId);
        Task HandlePaymentCancellation(PaymentIntent paymentIntent);
        Task HandlePaymentFailure(PaymentIntent paymentIntent);
    }
}
