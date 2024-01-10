using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Account
{
    public class HomeUserDto
    {
        public string? Username { get; set; } // "My Order"leri getirmek ucun istifade olunacag.
        public string? Fullname { get; set; }
        public string? Email { get; set; }
    }
}
