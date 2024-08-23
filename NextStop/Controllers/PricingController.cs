using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace NextStop.Controllers
{
    public class PricingController : BaseController
    {
        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentPrice()
        {
            try
            {
                var price = await _pricingService.GetCurrentPriceAsync();
                return Ok(price);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePrice([FromBody] decimal newPrice)
        {
            if (newPrice < 0)
            {
                return BadRequest("Price must be positive amount.");
            }

            try
            {
                await _pricingService.SetCurrentPriceAsync(newPrice);
                return Ok(new { Message = "Price updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }
    }
}
