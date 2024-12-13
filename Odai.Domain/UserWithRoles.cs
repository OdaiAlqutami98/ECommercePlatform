using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain
{
    public class UserWithRoles
    {
        public ApplicationUser User { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
