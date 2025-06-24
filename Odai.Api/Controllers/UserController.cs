using ECommercePlatform.Domain.Identity;
using ECommercePlatform.Logic.Services.IdentityUser;
using ECommercePlatform.Shared.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Odai.DataModel;
using Odai.Shared.Auth;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly SignInManager<User> _signInManager;
        public UserController(IIdentityService identityService, SignInManager<User> signInManager)
        {
            _identityService = identityService;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var respone = await _identityService.Register(model);
            if (respone.Succeeded)
            {
                return Ok(respone);
            }
            return BadRequest(respone);
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequstt request)
        {
            var response = await _identityService.AuthenticateAsync(request);
            if (response.Succeeded)
            {
                SetTokenCookie(response.Data.Token);
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();

        }
        [HttpGet]
        [Route("RegisterAdministrator")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdministrator()
        {
            var res = await _identityService.CreateUserAsync("Owner@OdaiShop.com", "P@ssw0rd");
            return Ok(res);
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

    }
}
