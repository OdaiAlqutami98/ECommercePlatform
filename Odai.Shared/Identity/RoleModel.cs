using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommercePlatform.Shared.Identity
{
    public class RoleModel
    {
        public Guid? Id { get; set; }
        public string NormalizedName { get; set; }
        public string Name { get; set; }
    }
}
