using FluentValidation;
using ServiceLayer.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.Account
{
    public class UserCreateValidation : AbstractValidator<UserCreateDto>
    {
        public UserCreateValidation()
        {
            RuleFor(x => x.Fullname)
           .NotNull().WithMessage("Ad soyad null ola bilməz.")
           .NotEmpty().WithMessage("Ad soyad boş ola bilməz.");

            RuleFor(x => x.Username)
                .NotNull().WithMessage("İstifadəçi adı null ola bilməz.")
                .NotEmpty().WithMessage("İstifadəçi adı boş ola bilməz.");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("E-poçt ünvanı null ola bilməz.")
                .NotEmpty().WithMessage("E-poçt ünvanı boş ola bilməz.")
                .EmailAddress().WithMessage("Düzgün e-poçt ünvanı daxil edilməyib.");

            RuleFor(x => x.Number)
                .NotNull().WithMessage("Nömrə null ola bilməz.")
                .NotEmpty().WithMessage("Nömrə boş ola bilməz.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Şifrə null ola bilməz.")
                .NotEmpty().WithMessage("Şifrə boş ola bilməz.")
                .MinimumLength(6).WithMessage("Şifrə ən az 6 simvoldan ibarət olmalıdır.");

            RuleFor(x => x.ConfrimPassword)
                .NotNull().WithMessage("Şifrə təsdiq null ola bilməz.")
                .NotEmpty().WithMessage("Şifrə təsdiq boş ola bilməz.")
                .Equal(x => x.Password).WithMessage("Şifrə ilə təsdiq şifrə uyğun gəlmir.");
        }
    }
}
