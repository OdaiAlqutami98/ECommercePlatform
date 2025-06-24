using ECommercePlatform.Domain.Entities;
using Odai.DataModel;
using Odai.Logic.Common;

namespace ECommercePlatform.Logic.Manager
{
    public class UserTypeManager : BaseServiceIdentity<UserType, OdaiDbContext>
    {
        public UserTypeManager(OdaiDbContext context) : base(context)
        {
        }
    }
}
