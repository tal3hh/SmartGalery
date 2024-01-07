using FluentValidation;
using ServiceLayer.Dtos.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class ProductImageCreateValidation : AbstractValidator<ProductImageCreateDto>
    {
        public ProductImageCreateValidation()
        {
            RuleFor(image => image.Path)
            .NotEmpty().WithMessage("Şəkil yol boş ola bilməz")
            .NotNull().WithMessage("Şəkil yol boş ola bilməz")
            .MaximumLength(255).WithMessage("Şəkil yolunun uzunluğu 255 simvoldan çox ola bilməz");

            RuleFor(detail => detail.ProductId)
                .NotEmpty().WithMessage("Məhsul ID boş ola bilməz")
                .NotNull().WithMessage("Məhsul ID boş ola bilməz")
                .GreaterThan(0).WithMessage("Məhsul ID 0-dan böyük olmalıdır");
        }
    }
}
