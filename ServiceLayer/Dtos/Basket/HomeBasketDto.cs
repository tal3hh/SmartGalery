using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Basket
{
    public class HomeBasketDto
    {
        public int productId { get; set; }
        public string? ProductPath { get; set; }
        public string? About { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
