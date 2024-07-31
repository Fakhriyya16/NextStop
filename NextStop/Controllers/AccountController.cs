using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Accounts;
using Service.Interfaces;

namespace NextStop.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("User ID and token are required.");
            }

            try
            {
                var response = await _accountService.ConfirmEmail(userId, token);
                if (response.StatusCode == (int)StatusCodes.Status200OK)
                {
                    return Ok(response.Message);
                }

                if (response.StatusCode == (int)StatusCodes.Status400BadRequest)
                {
                    return BadRequest(response.Errors);
                }

                return StatusCode(response.StatusCode, response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email is required.");
            }

            try
            {
                var response = await _accountService.ForgetPassword(email);
                if (response.StatusCode == (int)StatusCodes.Status404NotFound)
                {
                    return NotFound(response.Message);
                }

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _accountService.ResetPassword(model);

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

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromQuery] string id, [FromBody] UserUpdateDto request)
        {
            if (string.IsNullOrWhiteSpace(id) || request == null)
            {
                return BadRequest("Invalid request parameters.");
            }

            try
            {
                var response = await _accountService.UpdateProfile(id, request);
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

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                var response = await _accountService.LogOut();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

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
    }
}
