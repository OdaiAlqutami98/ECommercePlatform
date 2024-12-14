using Microsoft.AspNetCore.Identity;
using Odai.Domain.Enums;

namespace Odai.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Role Role { get; set; }
        public ICollection<Order>? Orders { get; set; }

    }
}
