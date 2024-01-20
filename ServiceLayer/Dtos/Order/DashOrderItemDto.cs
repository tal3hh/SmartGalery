using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Order
{
    public class DashOrderItemDto
    {
        public string? ProductName { get; set; }
        public string? ByUsername { get; set; }
        public int Quantity { get; set; }
        public decimal UnitePrice { get; set; }
    }
}
