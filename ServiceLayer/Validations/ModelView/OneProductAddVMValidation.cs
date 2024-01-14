using FluentValidation;
using ServiceLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.ModelView
{
    public class OneProductAddVMValidation : AbstractValidator<OneProductAddVM>
    {
        public OneProductAddVMValidation()
        {
            RuleFor(x => x.Username)
                .NotNull().WithMessage("İstifadəçi adı mütləqdir.")
                .NotEmpty().WithMessage("İstifadəçi adı mütləqdir.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId sıfırdan böyük olmalıdır.");
        }
    }
}
