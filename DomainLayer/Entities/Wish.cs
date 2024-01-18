using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Wish : BaseEntity
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
