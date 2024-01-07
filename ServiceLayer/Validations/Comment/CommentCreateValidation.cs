using FluentValidation;
using ServiceLayer.Dtos.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class CommentCreateValidation : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad boş ola bilməz.")
                .NotNull().WithMessage("Ad boş ola bilməz.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad boş ola bilməz.")
                .NotNull().WithMessage("Soyad boş ola bilməz.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-poçt boş ola bilməz.")
                .NotNull().WithMessage("E-poçt boş ola bilməz.")
                .EmailAddress().WithMessage("Düzgün e-poçt adresi daxil edin.");
        }
    }
}
