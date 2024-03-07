using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Order : BaseEntity
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public decimal TotalAmount { get; set; }

        //public int OrderStatusId { get; set; }
        //public OrderStatus OrderStatus { get; set; }

        public ShippingAsdress? ShippingAsdress { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
