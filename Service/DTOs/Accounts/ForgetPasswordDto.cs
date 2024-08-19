
using FluentValidation;

namespace Service.DTOs.Accounts
{
    public class ForgetPasswordDto
    {
        public string Email { get; set; }
    }

    public class ForgetPasswordDtoValidator: AbstractValidator<ForgetPasswordDto>
    {
        public ForgetPasswordDtoValidator()
        {
            RuleFor(m=>m.Email).NotEmpty().EmailAddress();
        }
    }
}
