using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.ProductImage
{
    public class ProductImageCreateDto
    {
        public string? Path { get; set; }

        public int ProductId { get; set; }
    }
}
