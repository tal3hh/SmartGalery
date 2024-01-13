using FluentValidation;
using ServiceLayer.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.Account
{
    public class RoleCreateValidation : AbstractValidator<RoleCreateDto>
    {
        public RoleCreateValidation()
        {
            RuleFor(x => x.Name)
           .NotNull().WithMessage("Ad null ola bilməz.")
           .NotEmpty().WithMessage("Ad boş ola bilməz.");
        }
    }
}
