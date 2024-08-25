using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace NextStop.Controllers
{
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
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

        [HttpGet]
        public async Task<IActionResult> GetAllNames()
        {
            try
            {
                var tags = await _tagService.GetAllNames();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
