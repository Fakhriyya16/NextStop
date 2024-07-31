
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Blogs
{
    public class BlogCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string AppUserId { get; set; }
        public List<IFormFile> Images { get; set; }
    }

    public class BlogCreateDtoValidator : AbstractValidator<BlogCreateDto>
    {
        public BlogCreateDtoValidator()
        {
            RuleFor(m => m.Title).NotEmpty();
            RuleFor(m => m.Content).NotEmpty();
            RuleFor(m => m.AppUserId).NotEmpty();
            RuleFor(m => m.Images).NotEmpty();
        }
    }
}
