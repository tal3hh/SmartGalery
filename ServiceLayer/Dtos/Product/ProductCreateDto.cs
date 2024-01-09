using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Product
{
    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsStock { get; set; }
        public string? Color { get; set; }
        public string? About { get; set; }
        
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
