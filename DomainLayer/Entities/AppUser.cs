using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Fullname { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public PasswordResetToken? PasswordResetToken { get; set; }
        public List<Rating>? Ratings { get; set; }
        public List<Basket>? Baskets { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Wish>? Wishes { get; set; }
        public List<ShippingAsdress>? ShippingAsdresses { get; set; }
    }
}
