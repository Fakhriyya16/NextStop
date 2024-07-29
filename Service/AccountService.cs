
using AutoMapper;
using CloudinaryDotNet;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Service.DTOs.Accounts;
using Service.Helpers;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly ISendEmail _sendEmail;

        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper, IUrlHelper urlHelper, ISendEmail sendEmail)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _sendEmail = sendEmail;
        }

        public Task<AccountManagementResponse> Login(LoginDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountManagementResponse> Register(RegisterDto model)
        {
            AppUser user = _mapper.Map<AppUser>(model);

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Errors = result.Errors.Select(m  => m.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, "Member");

            string confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = _urlHelper.Action("ConfirmEmail", "Account", new { userId = user.Id, token = confirmToken }, "https");

            var messageBody = string.Empty;

            using (StreamReader fileStream = new StreamReader("wwwroot/html/VerifyEmail.html"))
            {
                messageBody = await fileStream.ReadToEndAsync();
            }

            await _sendEmail.SendEmailAsync(user.Email, "Please Confirm Your Email", messageBody, isHtml: true);

            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = "Please verify your email."
            };
        }
    }
}
