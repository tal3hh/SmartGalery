using FluentValidation;
using ServiceLayer.Dtos.Brand;
using ServiceLayer.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.Brand
{
    public class BrandCreateValidation : AbstractValidator<BrandCreateDto>
    {
        public BrandCreateValidation()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Ad mütləqdir.")
                .NotEmpty().WithMessage("Ad mütləqdir.");
        }
    }
}
