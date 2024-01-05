using FluentValidation;
using ServiceLayer.Dtos.Subscribe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class SubscribeCreateValidation : AbstractValidator<SubscribeCreateDto>
    {
        public SubscribeCreateValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");
        }
    }
}
