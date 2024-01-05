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
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad boş olamaz.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");
            RuleFor(x => x.Number).NotEmpty().WithMessage("Numara boş olamaz.");
        }
    }
}
