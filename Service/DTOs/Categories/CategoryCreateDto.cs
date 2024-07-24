
using FluentValidation;

namespace Service.DTOs.Categories
{
    public class CategoryCreateDto 
    {
        public string Name { get; set; }
    }

    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
