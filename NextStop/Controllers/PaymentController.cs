using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Payments;
using Service.Interfaces;
using Stripe;

namespace NextStop.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;   
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentRequest request)
        {
            if (request == null || request.Amount <= 0 || string.IsNullOrWhiteSpace(request.Currency))
            {
                return BadRequest("Invalid payment intent request.");
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized("User ID not found.");
                }

                var payment = await _paymentService.CreatePaymentAsync(request, userId);
                return Ok(new { ClientSecret = payment.StripePaymentId, Payment = payment });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ParseEvent(json);

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    await _paymentService.ChangeSubscriptionType(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
                    break;

                case Events.PaymentIntentPaymentFailed:
                    var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    await _paymentService.HandlePaymentFailure(failedPaymentIntent);
                    break;

                case Events.PaymentIntentCanceled:
                    var canceledPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    await _paymentService.HandlePaymentCancellation(canceledPaymentIntent);
                    break;

                default:
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }

            return Ok();
        }

    }
}
