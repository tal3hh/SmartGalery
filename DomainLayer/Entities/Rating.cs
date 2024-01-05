using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Rating : BaseEntity
    {
        public decimal Star { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
