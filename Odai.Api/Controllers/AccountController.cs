using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Odai.Domain;
using Odai.Domain.Enums;
using Odai.Logic.Common.Interface;
using Odai.Shared;
using Odai.Shared.Auth;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IIdentityService identityService, UserManager<ApplicationUser> userManager)
        {
            _identityService= identityService;
            _userManager= userManager;
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
