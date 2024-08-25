using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Accounts;
using Service.Helpers;
using Service.Helpers.Exceptions;
using Service.Interfaces;
using System.Security.Claims;

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
        [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> GetAuthStatus()
        {
            var user = await _accountService.GetAuthenticatedUser(HttpContext);

            if (user != null)
            {
                return Ok(new
                {
                    IsLoggedIn = true,
                    User = new
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        Email = user.Email,
                        Subscription = user.Subscription.SubscriptionType
                    }
                });
            }
            else
            {
                return Ok(new { IsLoggedIn = false });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _accountService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _accountService.Register(request);

                if (response.StatusCode == (int)StatusCodes.Status400BadRequest)
                {
                    return BadRequest(response);
                }

                if (response.StatusCode == (int)StatusCodes.Status500InternalServerError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _accountService.Login(request);

                if (response.StatusCode == (int)StatusCodes.Status400BadRequest)
                {
                    return BadRequest(response);
                }

                if (response.StatusCode == (int)StatusCodes.Status404NotFound)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
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

        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _accountService.ForgetPassword(request.Email);
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromQuery] string userId)
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
