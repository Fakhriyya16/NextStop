
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Blogs
{
    public class BlogEditDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<IFormFile>? NewImages { get; set; }
    }

    public class BlogEditDtoValidator : AbstractValidator<BlogEditDto>
    {
        public BlogEditDtoValidator()
        {
            RuleFor(m=>m.Title).NotEmpty();
            RuleFor(m=>m.Content).NotEmpty();
        }
    }
}
