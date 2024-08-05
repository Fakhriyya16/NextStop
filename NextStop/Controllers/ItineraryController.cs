using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Itineraries;
using Service.Helpers.Exceptions;
using Service.Interfaces;
using System.Security.Claims;

namespace NextStop.Controllers
{
    public class ItineraryController : BaseController
    {
        private readonly IItineraryService _itineraryService;

        public ItineraryController(IItineraryService itineraryService)
        {
            _itineraryService = itineraryService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GenerateItinerary([FromBody] ItineraryRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.City) || request.NumberOfDays <= 0)
            {
                return BadRequest("Invalid itinerary request.");
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized("User must be logged in to generate an itinerary.");
                }

                var itinerary = await _itineraryService.GenerateItinerary(request, userId);
                return Ok(itinerary);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

    }
}
