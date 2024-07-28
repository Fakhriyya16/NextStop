
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Places
{
    public class PlaceCreateDto
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<int> TagIds { get; set; }
    }

    public class PlaceCreateDtoValidator : AbstractValidator<PlaceCreateDto>
    {
        public PlaceCreateDtoValidator()
        {
            RuleFor(m=>m.Name).NotEmpty();
            RuleFor(m=>m.CityId).NotEmpty();
            RuleFor(m=>m.CategoryId).NotEmpty();
            RuleFor(m=>m.Description).NotEmpty();
            RuleFor(m=>m.Images).NotEmpty();
            RuleFor(m=>m.TagIds).NotEmpty();
        }
    }
}
