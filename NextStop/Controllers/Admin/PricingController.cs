using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Payments;
using Service.Interfaces;

namespace NextStop.Controllers.Admin
{
    public class PricingController : BaseAdminController
    {
        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrice([FromBody] PriceUpdateDto priceUpdateDto)
        {
            if (priceUpdateDto.NewPrice < 0)
            {
                return BadRequest("Price must be a positive amount.");
            }

            try
            {
                await _pricingService.SetCurrentPriceAsync(priceUpdateDto.NewPrice);
                return Ok(new { Message = "Price updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
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
