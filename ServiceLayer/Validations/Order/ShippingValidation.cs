using FluentValidation;
using ServiceLayer.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.Order
{
    public class ShippingValidation : AbstractValidator<ShippingDto>
    {
        public ShippingValidation()
        {
            RuleFor(dto => dto.Username).NotNull().WithMessage("İstifadəçi adı mütləqdir.")
            .NotEmpty().WithMessage("İstifadəçi adı mütləqdir.");

            RuleFor(dto => dto.Email).NotNull().NotEmpty()
                .EmailAddress().WithMessage("Zəhmət olmasa etibarlı bir email ünvanı daxil edin.");

            RuleFor(dto => dto.Firstname).NotNull().WithMessage("Adınızı daxil edin.")
                .NotEmpty().WithMessage("Adınızı daxil edin.");

            RuleFor(dto => dto.Lastname).NotNull().WithMessage("Soyadınızı daxil edin.")
                .NotEmpty().WithMessage("Soyadınızı daxil edin.");

            RuleFor(dto => dto.StreetAddress).NotNull().WithMessage("Ünvanı daxil edin.")
                .NotEmpty().WithMessage("Ünvanı daxil edin.");

            RuleFor(dto => dto.City).NotNull().WithMessage("Şəhəri daxil edin.")
                .NotEmpty().WithMessage("Şəhəri daxil edin.");

            RuleFor(dto => dto.State).NotNull().WithMessage("State daxil edin.")
                .NotEmpty().WithMessage("State daxil edin.");

            RuleFor(dto => dto.PhoneNumber).NotNull().WithMessage("Zəhmət olmasa etibarlı bir telefon nömrəsi daxil edin.")
                .NotEmpty().WithMessage("Zəhmət olmasa etibarlı bir telefon nömrəsi daxil edin.");
        }
    }
}
