using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odai.DataModel;
using Odai.Logic.Common;

namespace ECommercePlatform.Logic.Services.Role
{
    public class RoleService : BaseServiceIdentity<Domain.Identity.Role, OdaiDbContext>
    {
        public RoleService(OdaiDbContext context) : base(context)
        {
        }
    }
}
