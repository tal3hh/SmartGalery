using FluentValidation;
using ServiceLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.ModelView
{
    public class ManyProductAddVMValidation : AbstractValidator<ManyProductAddVM>
    {
        public ManyProductAddVMValidation()
        {
            RuleFor(x => x.Username)
            .NotNull().WithMessage("İstifadəçi adı mütləqdir.")
            .NotEmpty().WithMessage("İstifadəçi adı mütləqdir.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId sıfırdan böyük olmalıdır.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miqdar sıfırdan böyük olmalıdır.")
                .LessThanOrEqualTo(10).WithMessage("Miqdar 10-dan az və ya bərabər ola bilər."); ;
        }
    }
}
