
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Service.DTOs.Cities
{
    public class CityCreateDto
    {
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }

    public class CityCreateDtoValidator : AbstractValidator<CityCreateDto>
    {
        public CityCreateDtoValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.CountryName).NotEmpty();
            RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.Image).NotEmpty();
        }
    }
}
