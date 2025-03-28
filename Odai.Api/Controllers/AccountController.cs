using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odai.Logic.Common.Interface;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        public AccountController(IIdentityService identityService)
        {
            _identityService= identityService;
        }
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult>GetUserById(Guid id)
        {
            var user =await _identityService.GetUserAsync(id);
            return Ok(user);
        }
        [HttpGet]
        [Route("GetAllUsers")]
      
        public async Task<IActionResult> GetAllUser()
        {
            var users=await _identityService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task <IActionResult> DeleteUser(Guid id)
        {
            var user= await _identityService.DeleteUser(id);
            return Ok(user);
        }
        [HttpPost]
        [Route("UpdateUserRoles")]
        public async Task<IActionResult> UpdateUserRoles(Guid userId, [FromBody] List<string> roles)
        {
            var response=await _identityService.UpdateUserRolesAsync(userId, roles);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response.Errors);
        }
        [HttpGet]
        [Route("RegisterAdministrator")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdministrator()
        {
            var res = await _identityService.CreateUserAsync("Owner@OdaiShop.com", "P@ssw0rd");
             return Ok(res);
        }
    }
}
