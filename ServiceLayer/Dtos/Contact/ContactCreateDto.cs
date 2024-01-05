using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Contact
{
    public class ContactCreateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Number { get; set; }
        public string? Message { get; set; }
    }
}
