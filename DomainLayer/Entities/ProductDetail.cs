using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class ProductDetail : BaseEntity
    {
        public string? Description { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
