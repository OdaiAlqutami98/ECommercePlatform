using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Domain.Entities;
using ECommercePlatform.Logic.ICommonService;
using ECommercePlatform.Shared.Models;
using Odai.Shared.Auth;

namespace ECommercePlatform.Logic.Services.Setting
{
    public interface ISettingService:IBaseService<Settings>
    {
        Task<Response<SettingsModel>> AddSetting(SettingsModel model);
        Task<Response<SettingsModel>> GetSetting();
    }
}
