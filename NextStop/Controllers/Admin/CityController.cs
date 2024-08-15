using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Cities;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace NextStop.Controllers.Admin
{
    public class CityController : BaseAdminController
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CityCreateDto request)
        {
            try
            {
                await _cityService.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (EntityExistsException ex)
            {
                return Conflict(new { message = ex.Message });
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
        public async Task<IActionResult> Edit([FromQuery] int id, [FromForm] CityEditDto request)
        {
            try
            {
                await _cityService.EditAsync(id, request);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
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
                await _cityService.DeleteAsync(id);
                return Ok();
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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cities = await _cityService.GetAllAsync();
                return Ok(cities);
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
                var city = await _cityService.GetByIdAsync(id);
                return Ok(city);
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
