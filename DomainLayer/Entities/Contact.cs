using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Contact : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Number { get; set; }
        public string? Message { get; set; }
    }
}
