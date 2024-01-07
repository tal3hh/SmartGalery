using FluentValidation;
using ServiceLayer.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class ProductCreateValidation : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidation()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad boş ola bilməz")
            .NotNull().WithMessage("Ad boş ola bilməz")
            .MaximumLength(50).WithMessage("Ad 50 simvoldan çox ola bilməz");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Qiymət 0-dan böyük olmalıdır");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Rəng boş ola bilməz")
                .NotNull().WithMessage("Rəng boş ola bilməz");

            RuleFor(x => x.About)
                .NotEmpty().WithMessage("Haqqinda boş ola bilməz")
                .NotNull().WithMessage("Haqqinda boş ola bilməz")
                .MaximumLength(200).WithMessage("Haqqında məlumat 200 simvoldan çox ola bilməz");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Kateqoriya ID 0-dan böyük olmalıdır");
        }
    }
}
