using Microsoft.AspNetCore.Mvc;
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedUsers([FromQuery] int currentPage, [FromQuery] int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _accountService.GetPaginatedUsers(currentPage, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving paginated users.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _accountService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser([FromQuery]string userId, [FromQuery] string role)
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

        [HttpPost]
        public async Task<IActionResult> RemoveRoleFromUser([FromQuery]string userId, [FromQuery] string role)
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
