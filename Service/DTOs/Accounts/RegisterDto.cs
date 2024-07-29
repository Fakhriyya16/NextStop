
using FluentValidation;

namespace Service.DTOs.Accounts
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(m=>m.Name).NotEmpty();
            RuleFor(m=>m.Surname).NotEmpty();
            RuleFor(m => m.Email).EmailAddress().NotEmpty();
            RuleFor(m => m.Password)
                    .MinimumLength(8)
                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            RuleFor(m => m.ConfirmPassword).NotEmpty().Equal(m=>m.Password).WithMessage("Passwords do not match.");
        }
    }
}
