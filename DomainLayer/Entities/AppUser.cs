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

        public List<Rating>? Ratings { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
