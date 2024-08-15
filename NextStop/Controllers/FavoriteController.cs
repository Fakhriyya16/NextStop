﻿using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace NextStop.Controllers
{
    public class FavoriteController : BaseController
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> CreateAsync([FromBody] Favorite model)
        {
            try
            {
                await _favoriteService.CreateAsync(model);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize] 
        public async Task<IActionResult> DeleteAsync([FromQuery]int id)
        {
            try
            {
                await _favoriteService.DeleteAsync(id);
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var favorites = await _favoriteService.GetAllAsync();
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPaginated(int currentPage, int pageSize)
        {
            try
            {
                var favorites = await _favoriteService.GetAllPaginated(currentPage, pageSize);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync([FromRoute]int id)
        {
            try
            {
                var favorite = await _favoriteService.GetByIdAsync(id);
                return Ok(favorite);
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
