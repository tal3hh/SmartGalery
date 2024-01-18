using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Wish
{
    public class HomeWishDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? ProductPath { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsStock { get; set; }
        public string? About { get; set; }
    }
}
