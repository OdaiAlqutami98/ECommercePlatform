using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common.Interface;
using Odai.Shared.Auth;
using Odai.Shared.Models;
using System.Security.Principal;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly OdaiDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(IIdentityService identityService, OdaiDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _identityService = identityService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ApplicationUserModel model)
        {
            var respone = await _identityService.RegisterUserAsync(model);
            if(respone.Succeeded)
            {
                return Ok(respone);

            }
            return BadRequest(respone);
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
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
         
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
