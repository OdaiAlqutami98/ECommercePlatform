using System.Security.Claims;
using ECommercePlatform.Logic.Services.RoleMenuItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleMenuItemsController(IRoleMenuItemsService _roleMenu) : ControllerBase
    {
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRolePagesAsync(Guid roleId)
        {
            var result = await _roleMenu.GetRolePagesAsync(roleId);

            if (result != null)
            {
                return Ok( result);
            }

            return BadRequest(false);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleRolePagesAsync([FromQuery] Guid roleId, [FromBody] List<int> pageIds)
        {
            var result = await _roleMenu.ToggleRolePagesAsync(roleId, pageIds);

            if (!result)
            {
                return BadRequest(false);
            }

            return Ok( result);
        }

        [HttpGet("GetRolesPages")]
        public async Task<IActionResult> GetRolesPages()
        {
            var userName = await GetCurrentUserNameAsync();
            var rolePages = await _roleMenu.getRolesPagesAsync(userName);

            if (rolePages == null)
            {
                return NotFound();
            }

            return Ok(rolePages);
        }

        private async Task<string> GetCurrentUserNameAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userName;
        }
    }
}
