using FluentValidation;
using ServiceLayer.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class CategoryCreateValidation : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateValidation()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Ad mütləqdir.")
                .NotEmpty().WithMessage("Ad mütləqdir.");
        }
    }
}
