using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Tags
{
    public class TagCreateDto
    {
        public string Name { get; set; }
    }

    public class TagCreateDtoValidator : AbstractValidator<TagCreateDto>
    {
        public TagCreateDtoValidator()
        {
            RuleFor(m=>m.Name).NotEmpty();
        }
    }
}
