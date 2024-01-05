using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Rating
{
    public class RatingUpdateDto
    {
        public int Id { get; set; }
        public decimal Star { get; set; }

        public int ProductId { get; set; }

        public string? AppUserId { get; set; }
    }
}
