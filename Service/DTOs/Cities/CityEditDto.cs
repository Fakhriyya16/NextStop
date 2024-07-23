
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Cities
{
    public class CityEditDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class CityEditDtoValidator : AbstractValidator<CityEditDto>
    {
        public CityEditDtoValidator()
        {
            RuleFor(m=>m.Name).NotEmpty();
            RuleFor(m=>m.Country).NotEmpty();
            RuleFor(m=>m.Description).NotEmpty();
        }
    }
}
