using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Order
{
    public class DashOrderDto
    {
        public string? Username { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
