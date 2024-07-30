
using AutoMapper;
using CloudinaryDotNet;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Service.DTOs.Accounts;
using Service.Helpers;
using Service.Helpers.Exceptions;
using Service.Interfaces;
using System.Web;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IUrlHelper _urlHelper;
        private readonly ISendEmail _sendEmail;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISubscriptionService _subscriptionService;
        private readonly string _baseUrl = "https://localhost:7264";

        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, 
                              SignInManager<AppUser> signInManager, ITokenService tokenService, 
                              IMapper mapper, IUrlHelperFactory urlHelperFactory, ISendEmail sendEmail, 
                              IHttpContextAccessor httpContextAccessor, ISubscriptionService subscriptionService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _urlHelperFactory = urlHelperFactory;
            _sendEmail = sendEmail;
            _httpContextAccessor = httpContextAccessor;
            _subscriptionService = subscriptionService;
        }

        public async Task<AccountManagementResponse> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status404NotFound,
                    Errors = new List<string> { "Invalid email or password" }
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (!result.Succeeded)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Errors = new List<string> { "Invalid email or password" }
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = _tokenService.GetToken(user, roles)
            };
        }

        public async Task<AccountManagementResponse> Register(RegisterDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Message = "Email is already taken"
                };
            }

            var user = _mapper.Map<AppUser>(model);
            user.UserName = model.Email;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Errors = result.Errors.Select(m => m.Description).ToList()
                };
            }

            await _subscriptionService.CreateAsync(new Subscription
            {
                AppUserId = user.Id,
                SubscriptionType = "Free",
                IsActive = true
            });

            await _userManager.AddToRoleAsync(user, "Member");

            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{_baseUrl}/api/account/confirmemail?userId={HttpUtility.UrlEncode(user.Id)}&token={HttpUtility.UrlEncode(confirmToken)}";

            string messageBody;
            try
            {
                using (var fileStream = new StreamReader("wwwroot/html/VerifyEmail.html"))
                {
                    messageBody = await fileStream.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status500InternalServerError,
                    Message = "Error reading email template."
                };
            }

            messageBody = messageBody.Replace("{{verificationLink}}", confirmationLink)
                                     .Replace("{{userName}}", $"{user.Name} {user.Surname}");

            await _sendEmail.SendEmailAsync(user.Email, "Please Confirm Your Email", messageBody, true);

            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = "Please verify your email."
            };
        }

        public async Task<AccountManagementResponse> ConfirmEmail(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status404NotFound,
                    Message = "User not found"
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Errors = result.Errors.Select(m => m.Description).ToList()
                };
            }

            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = "Email confirmed successfully."
            };
        }

        public async Task<AccountManagementResponse> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Message = "Email is required."
                };
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status404NotFound,
                    Message = "User does not exist"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"{_baseUrl}/api/account/resetpassword?email={HttpUtility.UrlEncode(email)}&token={HttpUtility.UrlEncode(token)}";

            string messageBody;
            try
            {
                using (var fileStream = new StreamReader("wwwroot/html/ResetPassword.html"))
                {
                    messageBody = await fileStream.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status500InternalServerError,
                    Message = "Error reading email template."
                };
            }

            messageBody = messageBody.Replace("{{ResetLink}}", url)
                                     .Replace("{{Username}}", $"{user.Name} {user.Surname}");

            await _sendEmail.SendEmailAsync(email, "Reset Your Password", messageBody, true);

            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = token
            };
        }

        public async Task<AccountManagementResponse> ResetPassword(ResetPasswordDto model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Message = "Passwords do not match."
                };
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status404NotFound,
                    Message = "User does not exist"
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Errors = result.Errors.Select(m => m.Description).ToList()
                };
            }

            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = "Password has been reset successfully."
            };
        }

        public async Task CreateRoles()
        {
            string[] roles = { "Admin", "Member" };

            foreach (string role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);

                if (!roleExist) await _roleManager.CreateAsync(new IdentityRole { Name = role });
            }
        }

        public async Task<AccountManagementResponse> UpdateProfile(string userId,UserUpdateDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status404NotFound,
                    Message = "User not found"
                };
            }

            if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    return new AccountManagementResponse
                    {
                        StatusCode = (int)StatusCodes.Status400BadRequest,
                        Message = "New passwords do not match."
                    };
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    return new AccountManagementResponse
                    {
                        StatusCode = (int)StatusCodes.Status400BadRequest,
                        Errors = changePasswordResult.Errors.Select(e => e.Description).ToList()
                    };
                }
            }

            var updateResult = await _userManager.UpdateAsync(_mapper.Map(model, user));
            if (!updateResult.Succeeded)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Errors = updateResult.Errors.Select(e => e.Description).ToList()
                };
            }

            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = "Profile updated successfully!"
            };
        }

        public async Task<AccountManagementResponse> DeleteProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status404NotFound,
                    Message = "User not found"
                };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = "User account deleted successfully."
            };
        }

        public async Task<AccountManagementResponse> LogOut()
        {
            await _signInManager.SignOutAsync();
            return new AccountManagementResponse
            {
                StatusCode = (int)StatusCodes.Status200OK,
                Message = "User logged out successfully."
            };
        }
    }
}
