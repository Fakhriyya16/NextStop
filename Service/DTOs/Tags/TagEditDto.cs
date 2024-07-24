using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Tags
{
    public class TagEditDto
    {
        public string Name { get; set; }
    }

    public class TagEditDtoValidator : AbstractValidator<TagEditDto>
    {
        public TagEditDtoValidator()
        {
            RuleFor(m=>m.Name).NotEmpty();
        }
    }
}
