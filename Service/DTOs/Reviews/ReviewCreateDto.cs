
using FluentValidation;

namespace Service.DTOs.Reviews
{
    public class ReviewCreateDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class ReviewCreateDtoValidator : AbstractValidator<ReviewCreateDto>
    {
        public ReviewCreateDtoValidator()
        {
            RuleFor(m=>m.Rating).NotEmpty().InclusiveBetween(0,5);
            RuleFor(m => m.Comment).NotEmpty();
        }
    }
}
