using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Payments;
using Service.Helpers.Exceptions;
using Service.Interfaces;
using Stripe;
using System.Security.Claims;

namespace NextStop.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;   
        }

        [Authorize]
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

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> ChangeSubscription()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User is not authorized" });
                }

                await _paymentService.ChangeSubscriptionType(userId);
                return Ok(new { message = "Subscription changed successfully" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPaymentByUserId([FromQuery]string userId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByUserId(userId);
                return Ok(payment);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpPost("handle-cancellation")]
        public async Task<IActionResult> HandlePaymentCancellation([FromBody] PaymentIntent paymentIntent)
        {
            try
            {
                await _paymentService.HandlePaymentCancellation(paymentIntent);
                return Ok(new { message = "Payment cancellation handled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("handle-failure")]
        [AllowAnonymous] 
        public async Task<IActionResult> HandlePaymentFailure([FromBody] PaymentIntent paymentIntent)
        {
            try
            {
                await _paymentService.HandlePaymentFailure(paymentIntent);
                return Ok(new { message = "Payment failure handled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
