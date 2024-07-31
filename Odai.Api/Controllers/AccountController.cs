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
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(IIdentityService identityService,UserManager<ApplicationUser> userManager)
        {
            _identityService = identityService;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ApplicationUserModel model)
        {
            var respone = await _identityService.RegisterUserAsync(model);
            return Ok(respone);
        }


        [HttpGet]
        [Route("GetUserById")]
        [AllowAnonymous]
        public async Task<IActionResult>GetUserById(Guid id)
        {
            var user =await _identityService.GetUserAsync(id);
            return Ok(user);
        }
        [HttpGet]
        [Route("GetAllUsers")]
        [AllowAnonymous]
       // [Authorize(Roles ="Owner")]
        public async Task<IActionResult> GetAllUser()
        {
            var users=await _identityService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequstt request)
        {
            var response = await _identityService.AuthenticateAsync(request);
            if (response.Succeeded)
            {
                SetTokenCookie(response.Data.Token);
            }
            return Ok(response);
        }
        private void SetTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        [HttpGet]
        [Route("GetUserRoles")]
       // [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var roles=_identityService.GetUserRolesAsync(userId);
            if (roles==null)
            {
                return BadRequest();
            }
            return Ok(roles);
        }
        [HttpPost]
        [Route("UpdateUserRoles")]
     //   [Authorize(Roles = "Owner")]
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
