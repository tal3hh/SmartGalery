using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Product.Home
{
    public class HomeProDetailDto
    {
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? About { get; set; }
        public decimal Price { get; set; }

        public List<string>? ProductImages { get; set; }
        public List<string>? ProductDetails { get; set; }
    }
}
