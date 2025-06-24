using System.Data;
using ECommercePlatform.Domain.Identity;
using ECommercePlatform.Logic.Services.Role;
using ECommercePlatform.Shared.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.Shared.Auth;
using Odai.Shared.Table;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _service;
        public RoleController(RoleService service)
        {
            _service = service;
        }
        [HttpGet("GetAll")]
        public async Task<TableResponse<Role>> GetAll(int pageIndex, int pageSize)
        {
            var query =  _service.GetAll();
            var length = query.Count();
            var role = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new TableResponse<Role>(role, length);
        }
        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole()
        {
            var result = await _service.GetAll().ToListAsync();
            if (result != null)
            {
                return Ok(result);

            }
           return  BadRequest(false);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            var roleId = Guid.Parse(id);
            var role = await _service.GetById(roleId);
            if (role is not null)
            {
                return Ok(role);
            }
            return BadRequest(false);
        }
        [HttpPost("AddEdit")]
        public async Task<IActionResult> AddEdit(RoleModel model)
        {
            
            if (model.Id == null)
            {
                Role role = new Role();
                role.Name = model.Name;
                role.NormalizedName = model.NormalizedName;
                await _service.Insert(role);
                await _service.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Role added successfully.", Data = true });
            }
            else
            {
                var role = await _service.Get(o => o.Id == model.Id).FirstOrDefaultAsync();

                if (role != null)
                {
                    role.Name = model.Name;
                    role.NormalizedName = model.NormalizedName;

                    _service.Update(role);
                    await _service.SaveChangesAsync();
                    return Ok(new Response<bool> { Succeeded = true, Message = "Role updated successfully.", Data = true });
                }

            }
            return BadRequest(false);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var roleId = Guid.Parse(id);

            var role = await _service.Get(o => o.Id == roleId).FirstOrDefaultAsync();
            if (role != null)
            { 
                await _service.Delete(role);
                await _service.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
            return NotFound(new Response<bool>());
        }
    }
}
