﻿using FluentValidation;
using ServiceLayer.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    internal class ProductUpdateValidation : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateValidation()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id mütləqdir.");
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad boş ola bilməz")
            .MaximumLength(50).WithMessage("Ad 50 simvoldan çox ola bilməz");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Qiymət 0-dan böyük olmalıdır");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Rəng boş ola bilməz");

            RuleFor(x => x.About)
                .MaximumLength(200).WithMessage("Haqqında məlumat 200 simvoldan çox ola bilməz");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kateqoriya ID boş ola bilməz")
                .GreaterThan(0).WithMessage("Kateqoriya ID 0-dan böyük olmalıdır");
        }
    }
}