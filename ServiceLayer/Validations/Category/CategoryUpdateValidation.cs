using FluentValidation;
using ServiceLayer.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class CategoryUpdateValidation : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateValidation()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id mütləqdir.");
            RuleFor(x => x.Name).NotNull().WithMessage("Ad mütləqdir.");
        }
    }
}
