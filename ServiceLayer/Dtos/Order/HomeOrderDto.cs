using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Order
{
    public class HomeOrderDto
    {
        public decimal Subtotal { get; set; }
        public List<HomeOrderItemDto>? HomeOrderItemDtos { get; set; }
    }
}
