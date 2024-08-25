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
    }
}
