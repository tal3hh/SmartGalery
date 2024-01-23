using FluentValidation;
using ServiceLayer.Dtos.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class ContactCreateValidation : AbstractValidator<ContactCreateDto>
    {
        public ContactCreateValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad boş ola bilməz.")
                .NotNull().WithMessage("Ad boş ola bilməz.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-poçt boş ola bilməz.")
                .NotNull().WithMessage("E-poçt boş ola bilməz.")
                .EmailAddress().WithMessage("Düzgün bir e-poçt ünvanı daxil edin.");

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Nömrə boş ola bilməz.")
                .NotNull().WithMessage("Nömrə boş ola bilməz.");

        }
    }
}
