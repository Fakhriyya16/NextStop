
using FluentValidation;

namespace Service.DTOs.Categories
{
    public class CategoryEditDto
    {
        public string Name { get; set; }
    }

    public class CategoryEditDtoValidator : AbstractValidator<CategoryEditDto>
    {
        public CategoryEditDtoValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
