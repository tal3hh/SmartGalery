using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public bool IsStock { get; set; }
        public string? Color { get; set; }
        public string? About { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public Rating? Rating { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ProductDetail>? ProductDetails { get; set; }
    }
}
