
using FluentValidation;
using Microsoft.AspNetCore.Http;


namespace Service.DTOs.Places
{
    public class PlaceEditDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<int> TagIds { get; set; }
    }

    public class PlaceEditDtoValidator : AbstractValidator<PlaceEditDto>
    {
        public PlaceEditDtoValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.CategoryId).NotEmpty();
            RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.TagIds).NotEmpty();
        }
    }
}
