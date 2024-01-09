using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsStock { get; set; }
        public string? Color { get; set; }
        public string? About { get; set; }
        public List<string>? ProductImages { get; set; }
        public List<string>? ProductDetails { get; set; }
        public int commentCount { get; set; }
        public decimal Rating { get; set; }

        public int CategoryId { get; set; }
    }
}
