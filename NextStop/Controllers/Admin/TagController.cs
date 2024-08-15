using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Tags;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace NextStop.Controllers.Admin
{
    public class TagController : BaseAdminController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagCreateDto request)
        {
            try
            {
                await _tagService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), request);
            }
            catch (EntityExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromQuery] int id, [FromBody] TagEditDto request)
        {
            try
            {
                await _tagService.EditAsync(id, request);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new { message = "Tag ID cannot be null" });
            }
            catch (EntityExistsException ex)
            {
                return Conflict(new { message = ex.Message });
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

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                await _tagService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new { message = "Tag ID cannot be null" });
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var tag = await _tagService.GetByIdAsync(id);
                return Ok(tag);
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
                var tags = await _tagService.GetAllAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
