using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Product.Home
{
    public class HomeProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsStock { get; set; }
        public string? About { get; set; }
        public List<string>? ProductImages { get; set; }
        public int commentCount { get; set; }
        public decimal Rating { get; set; }
    }
}
