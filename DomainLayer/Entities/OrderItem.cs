using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public string? ProductName { get; set; }
        public string? ByUsername { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
