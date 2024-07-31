using Microsoft.AspNetCore.Identity;
using Odai.Domain.Enums;

namespace Odai.Domain
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public UserType UserType { get; set; }
       
    }
}
