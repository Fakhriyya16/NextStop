
using FluentValidation;

namespace Service.DTOs.Countries
{
    public class CountryCreateDto
    {
        public string Name { get; set; }
    }

    public class CountryCreateDtoValidator : AbstractValidator<CountryCreateDto>
    {
        public CountryCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
