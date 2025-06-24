using ECommercePlatform.Api.Common;
using ECommercePlatform.Domain.Entities;
using ECommercePlatform.Logic.Services.Setting;
using ECommercePlatform.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : BaseController<Settings, ISettingService>
    {
        private readonly ISettingService _service;
        public SettingController(ISettingService service) : base(service)
        {
            _service = service;
        }

        [HttpGet("GetSetting")]
        public async Task<IActionResult> GetSetting()
        {
            var setting = await _service.GetSetting();
            if (setting != null)
            {
                return Ok(setting);
            }
            return BadRequest();
        }

        [HttpPost("AddSetting")]
        public async Task<IActionResult> AddSetting([FromForm]SettingsModel model)
        {
            var setting=await _service.AddSetting(model);
            if(setting != null)
            {
                return Ok(setting);
            }
            return BadRequest();
        }
       


    }
}
