using ECommercePlatform.Api.Common;
using ECommercePlatform.Domain.Entities;
using ECommercePlatform.Logic.Manager;
using ECommercePlatform.Logic.Services.UserType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : BaseController<UserType, IUserTypeService>
    {
        public UserTypeController(IUserTypeService service) : base(service)
        {
        }
    }
}
