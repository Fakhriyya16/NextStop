using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Accounts;
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

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            return Ok(await _accountService.Register(request));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            return Ok(await _accountService.Login(request));
        }
    }
}
