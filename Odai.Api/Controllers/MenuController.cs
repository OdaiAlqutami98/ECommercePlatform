using System.Net;
using ECommercePlatform.Logic.Services.Menu;
using ECommercePlatform.Shared.Constants;
using ECommercePlatform.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odai.Shared.Auth;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(IMenuService _menuService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPagesAsync()
        {
            var result = await _menuService.GetAllPagesAsync();

            if (result != null)
            {
                return Ok( result);

            }

            return BadRequest(false);
        }

        [HttpPost]
        public async Task<IActionResult> AddPageAsync(MenuModel page)
        {
            var result = await _menuService.AddPageAsync(page);

            if (!result)
            {
                return BadRequest();
            }

            return Ok( result);
        }
    }
}
