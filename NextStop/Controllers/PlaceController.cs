using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace NextStop.Controllers
{
    public class PlaceController : BaseController
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        public async Task<IActionResult> SortBy([FromQuery] string property, [FromQuery] string order, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            try
            {
                var places = await _placeService.SortBy(property, order,currentPage,pageSize);
                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchByName([FromQuery] string searchText, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            try
            {
                var places = await _placeService.SearchByName(searchText, currentPage, pageSize);
                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> FilterByCategory([FromQuery] string category, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            try
            {
                var places = await _placeService.FilterByCategory(category, currentPage, pageSize);
                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> FilterByCity([FromQuery] string city, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            try
            {
                var places = await _placeService.FilterByCity(city, currentPage, pageSize);
                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> FilterByTag([FromQuery] string tag, [FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            try
            {
                var places = await _placeService.FilterByTag(tag, currentPage, pageSize);
                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var places = await _placeService.GetAllAsync();
                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            try
            {
                var response = await _placeService.GetAllPaginated(currentPage, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var place = await _placeService.GetByIdAsync(id);
                return Ok(place);
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
    }
}
