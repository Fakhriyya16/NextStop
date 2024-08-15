using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Blogs;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace NextStop.Controllers.Admin
{
    public class BlogController : BaseAdminController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery]int? id)
        {
            try
            {
                await _blogService.DeleteAsync(id);
                return Ok("Blog deleted successfully.");
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
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var blogs = await _blogService.GetAllAsync();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var blog = await _blogService.GetByIdAsync(id);
                if (blog == null)
                    return NotFound(new { message = "Blog not found." });

                return Ok(blog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedBlogs([FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            try
            {
                var blogs = await _blogService.GetPaginatedBlogs(currentPage, pageSize);
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
