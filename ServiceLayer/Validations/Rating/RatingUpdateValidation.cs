using FluentValidation;
using ServiceLayer.Dtos.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations
{
    public class RatingUpdateValidation : AbstractValidator<RatingUpdateDto>
    {
        public RatingUpdateValidation()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id mütləqdir.");
            RuleFor(rating => rating.Star)
            .InclusiveBetween(1, 5).WithMessage("Reytinq 1 ilə 5 arasında olmalıdır");

            RuleFor(rating => rating.ProductId)
                .NotEmpty().WithMessage("Məhsul ID boş ola bilməz")
                .GreaterThan(0).WithMessage("Məhsul ID 0-dan böyük olmalıdır");

            RuleFor(rating => rating.AppUserId)
                .NotEmpty().WithMessage("İstifadəçi ID boş ola bilməz");
        }
    }
}
