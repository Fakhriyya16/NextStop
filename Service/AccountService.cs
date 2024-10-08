﻿using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Helpers;
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
        private readonly ISendEmail _sendEmail;
        private readonly ISubscriptionService _subscriptionService;
        private readonly string _baseUrl = "https://localhost:7264";
        private readonly string _baseUiUrl = "http://localhost:3000";

        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, 
                              SignInManager<AppUser> signInManager, ITokenService tokenService, 
                              IMapper mapper, ISendEmail sendEmail, 
                              ISubscriptionService subscriptionService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _sendEmail = sendEmail;
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
            var confirmationLink = $"{_baseUiUrl}/confirmemail?userId={HttpUtility.UrlEncode(user.Id)}&token={HttpUtility.UrlEncode(confirmToken)}";

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
            var url = $"{_baseUiUrl}/resetpassword?email={HttpUtility.UrlEncode(email)}&token={HttpUtility.UrlEncode(token)}";

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
            var subscription = await _subscriptionService.GetByUserId(userId);

            await _subscriptionService.DeleteFromDatabase(subscription);

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

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDetailDto> GetUserById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var userDetail = _mapper.Map<UserDetailDto>(user);
            userDetail.Roles = await _userManager.GetRolesAsync(user);
            userDetail.SubscriptionType = _subscriptionService.GetByUserId(id).Result.SubscriptionType;
            return userDetail;
        }

        public async Task<AccountManagementResponse> AddRoleToUser(string userId, string role)
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

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(role))
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Message = "User already has this role"
                };
            }

            var result = await _userManager.AddToRoleAsync(user, role);
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
                Message = "Role added successfully"
            };
        }

        public async Task<AccountManagementResponse> RemoveRoleFromUser(string userId, string role)
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

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(role))
            {
                return new AccountManagementResponse
                {
                    StatusCode = (int)StatusCodes.Status400BadRequest,
                    Message = "User does not have this role"
                };
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
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
                Message = "Role removed successfully"
            };
        }

        public async Task<PaginationResponse<UserDto>> GetPaginatedUsers(int currentPage,int pageSize)
        {
            var totalCount = await _userManager.Users.CountAsync();

            int pageCount = (int)Math.Ceiling((double)(totalCount / pageSize));

            var data = await _userManager.Users.OrderBy(m => m.Surname)
                                         .Skip((currentPage - 1) * pageSize)
                                         .Take(pageSize).ToListAsync();

            var mappedData = _mapper.Map<List<UserDto>>(data);

            foreach (var u in mappedData)
            {
                var user = await _userManager.FindByIdAsync(u.Id);
                u.Roles = (List<string>)await _userManager.GetRolesAsync(user);
            }

            bool hasNext = true;
            bool hasPrevious = true;

            if (currentPage == 1)
            {
                hasPrevious = false;
            }
            if (currentPage == pageCount)
            {
                hasNext = false;
            }

            var response = new PaginationResponse<UserDto>()
            {
                Data = mappedData,
                TotalCount = totalCount,
                CurrentPage = currentPage,
                PageCount = pageCount,
                PageSize = pageSize,
                HasNext = hasNext,
                HasPrevious = hasPrevious,
            };

            return response;
        }

        public bool IsAuthenticated(HttpContext httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated;
        }

        public async Task<AppUser> GetAuthenticatedUser(HttpContext httpContext)
        {
            if (IsAuthenticated(httpContext))
            {
                var userId = _userManager.GetUserId(httpContext.User);
                return await _userManager.FindByIdAsync(userId);
            }
            return null;
        }
    }
}
