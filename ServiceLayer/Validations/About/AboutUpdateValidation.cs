using FluentValidation;
using ServiceLayer.Dtos.About;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class AboutUpdateValidation : AbstractValidator<AboutUpdateDto>
    {
        public AboutUpdateValidation()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("ID mütləqdir.");
            RuleFor(x => x.Title).NotNull().WithMessage("Başlıq mütləqdir.")
                .NotEmpty().WithMessage("Başlıq mütləqdir.");
            RuleFor(x => x.Description).NotNull().WithMessage("Açıqlama mütləqdir.")
                .NotEmpty().WithMessage("Açıqlama mütləqdir.");
        }
    }
}
