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
        public DateTime OrderDate { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }

        //public int OrderStatusId { get; set; }
        //public OrderStatus OrderStatus { get; set; }

        public List<OrderItem>? OrderItems { get; set; }

        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Company { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
    }
}
