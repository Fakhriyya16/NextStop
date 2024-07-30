
using FluentValidation;

namespace Service.DTOs.Accounts
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(m=>m.Name).NotEmpty();
            RuleFor(m=>m.Surname).NotEmpty();
        }
    }
}
