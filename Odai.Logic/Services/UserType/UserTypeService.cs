using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Logic.CommonService;
using Odai.DataModel;

namespace ECommercePlatform.Logic.Services.UserType
{
    public class UserTypeService:BaseService<Domain.Entities.UserType>,IUserTypeService
    {
        public UserTypeService(OdaiDbContext context) : base(context)
        {
        }
    }
}
