
using FluentValidation;

namespace Service.DTOs.Countries
{
    public class CountryEditDto
    {
        public string Name { get; set; }
    }

    public class CountryEditDtoValidator : AbstractValidator<CountryEditDto>
    {
        public CountryEditDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
