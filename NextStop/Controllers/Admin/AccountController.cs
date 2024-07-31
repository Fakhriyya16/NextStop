using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Accounts;
using Service.Helpers;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace NextStop.Controllers.Admin
{
    public class AccountController : BaseAdminController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _accountService.CreateRoles();
        //    return Ok();
        //}

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromBody] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var response = await _accountService.DeleteProfile(userId);
                if (response.StatusCode == (int)StatusCodes.Status404NotFound)
                {
                    return NotFound(response.Message);
                }

                if (response.StatusCode == (int)StatusCodes.Status400BadRequest)
                {
                    return BadRequest(response);
                }

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _accountService.GetAllUsers());
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(string userId, [FromBody] string role)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("User ID and role are required.");
            }

            try
            {
                var response = await _accountService.AddRoleToUser(userId, role);

                if (response.StatusCode == (int)StatusCodes.Status404NotFound)
                {
                    return NotFound(response.Message);
                }

                if (response.StatusCode == (int)StatusCodes.Status400BadRequest)
                {
                    return BadRequest(response.Message);
                }

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveRoleFromUser(string userId, [FromBody] string role)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("User ID and role are required.");
            }

            try
            {
                var response = await _accountService.RemoveRoleFromUser(userId, role);

                if (response.StatusCode == (int)StatusCodes.Status404NotFound)
                {
                    return NotFound(response.Message);
                }

                if (response.StatusCode == (int)StatusCodes.Status400BadRequest)
                {
                    return BadRequest(response.Message);
                }

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersDetails([FromQuery] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var userDetail = await _accountService.GetUserById(id);
                return Ok(userDetail);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }   
}
