using FluentValidation;
using ServiceLayer.Dtos.About;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class AboutCreateValidation : AbstractValidator<AboutCreateDto>
    {
        public AboutCreateValidation()
        {
            RuleFor(x => x.Title).NotNull().WithMessage("Başlıq mütləqdir.");
            RuleFor(x => x.Description).NotNull().WithMessage("Açıqlama mütləqdir.");
        }
    }
}
