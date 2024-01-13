using FluentValidation;
using ServiceLayer.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.Account
{
    public class UserLoginValidation : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidation()
        {
            RuleFor(x => x.UsernameorEmail)
            .NotNull().WithMessage("İstifadəçi adı və ya e-poçt ünvanı null ola bilməz.")
            .NotEmpty().WithMessage("İstifadəçi adı və ya e-poçt ünvanı boş ola bilməz.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Şifrə null ola bilməz.")
                .NotEmpty().WithMessage("Şifrə boş ola bilməz.");
        }
    }
}
