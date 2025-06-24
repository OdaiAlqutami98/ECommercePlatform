using ECommercePlatform.Domain.Identity;
using ECommercePlatform.Logic.Services.IdentityUser;
using ECommercePlatform.Shared.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.Shared.Table;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly UserManager<User> _userManager;
        public AuthenticateController(IIdentityService identityService, UserManager<User> userManager)
        {
            _identityService = identityService;
            _userManager = userManager;
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser(int pageIndex, int pageSize)
        {
            var user = _userManager.Users.Where(u => u.IsDeleted == 0);
            var totalUser = await user.CountAsync();
            var query = await user.Skip(pageIndex  * pageSize).Take(pageSize).Select(u => new RegisterModel
            {
                Id=u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                UserTypeId = u.UserTypeId,
            }).ToListAsync();
            var response = new TableResponse<RegisterModel>(query, totalUser);
            return Ok(response);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var Id = Guid.Parse(id);
            var user = await _identityService.GetUserAsync(Id);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();

        }

        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterModel model)
        {
            var respone = await _identityService.RegisterAdmin(model);
            if (respone.Succeeded)
            {
                return Ok(respone);
            }
            return BadRequest(respone);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> Update(RegisterModel model)
        {
            var user = await _identityService.UpdateUser(model);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpGet("FindUserRoles")]
        public async Task<IActionResult> FindUserRoles(string userName)
        {
            var result = await _identityService.FindUserRolesAsync(userName);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(404);
        }
        
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var Id = Guid.Parse(id);
            var user = await _identityService.DeleteUser(Id);
            if (user != null)
            {
                return Ok(user);

            }
            return BadRequest();
        }
    }
}
