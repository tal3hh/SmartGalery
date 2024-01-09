using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Product.Dash
{
    public class DashProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsStock { get; set; }
        public string? Color { get; set; }
        public string? About { get; set; }
        public int Count { get; set; }

        public List<string>? ProductImages { get; set; }
        public List<string>? ProductDetails { get; set; }
        
        public int commentCount { get; set; }
        public decimal Rating { get; set; }

        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }

        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
