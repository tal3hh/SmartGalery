using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class PasswordResetToken : BaseEntity
    {
        public string? AppUserId { get; set; }
        public string? Token { get; set; }
        public bool IsExpired => DateTime.UtcNow > CreateDate.AddHours(1);

        public AppUser? AppUser { get; set; }
    }
}
