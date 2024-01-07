using FluentValidation;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Dtos.ProductDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class ProductDetailCreateValidation : AbstractValidator<ProductDetailCreateDto>
    {
        public ProductDetailCreateValidation()
        {
            RuleFor(detail => detail.Description)
           .NotEmpty().WithMessage("Məlumat boş ola bilməz")
           .NotNull().WithMessage("Məlumat boş ola bilməz")
           .MaximumLength(250).WithMessage("Məlumat 250 simvoldan çox ola bilməz");

            RuleFor(detail => detail.ProductId)
                .NotEmpty().WithMessage("Məhsul ID boş ola bilməz")
                .GreaterThan(0).WithMessage("Məhsul ID 0-dan böyük olmalıdır");
        }
    }
}
