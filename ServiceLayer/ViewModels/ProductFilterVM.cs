using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ViewModels
{
    public class ProductFilterVM
    {
        public int CategoryId { get; set; }
        public decimal PriceMIN { get; set; }
        public decimal PriceMAX { get; set; }
        public string? Color { get; set; }
        public int Page { get; set; }   
        public int Take { get; set; }
    }
}
