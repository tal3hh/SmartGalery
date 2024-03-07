using FluentValidation;
using ServiceLayer.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.Account
{
    public class UserResetPassValidation : AbstractValidator<UserResetPassDto>
    {
        public UserResetPassValidation()
        {
            RuleFor(x => x.Username)
            .NotNull().WithMessage("İstifadəçi adı mütləqdir.")
            .NotEmpty().WithMessage("İstifadəçi adı boş ola bilməz.");

            RuleFor(x => x.OldPassword)
                .NotNull().WithMessage("Köhnə Şifrə mütləqdir.")
                .NotEmpty().WithMessage("Köhnə Şifrə boş ola bilməz.");

            RuleFor(x => x.NewPassword)
                .NotNull().WithMessage("Yeni Şifrə mütləqdir.")
                .NotEmpty().WithMessage("Yeni Şifrə boş ola bilməz.")
                .MinimumLength(6).WithMessage("Yeni Şifrə ən azı 6 simvol olmalıdır.");

            RuleFor(x => x.NewConfrimPassword)
                .NotNull().WithMessage("Şifrəni Təsdiqlə mütləqdir.")
                .NotEmpty().WithMessage("Şifrəni Təsdiqlə boş ola bilməz.")
                .Equal(x => x.NewPassword).WithMessage("Şifrəni Təsdiqlə, Yeni Şifrə ilə eyni olmalıdır.");
        }
    }
}
