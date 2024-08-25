using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Places;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace NextStop.Controllers.Admin
{
    public class PlaceController : BaseAdminController
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PlaceCreateDto request)
        {
            try
            {
                await _placeService.CreateAsync(request);
                return Ok();
            }
            catch (EntityExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidImageFormatException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (FileSizeExceededException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromQuery] int id, [FromForm] PlaceEditDto request)
        {
            try
            {
                await _placeService.EditAsync(id, request);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new { message = "Place ID cannot be null" });
            }
            catch (EntityExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidImageFormatException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (FileSizeExceededException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                await _placeService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new { message = "Place ID cannot be null" });
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
        public async Task<IActionResult> GetAllPaginated([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 8)
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
