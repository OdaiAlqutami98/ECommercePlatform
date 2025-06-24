using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Shared.Models;

namespace ECommercePlatform.Logic.Services.RoleMenuItems
{
    public interface IRoleMenuItemsService
    {
        Task<bool> ToggleRolePagesAsync(Guid roleId, List<int> pagesIds);
        Task<List<RoleMenuItemsModel>> GetRolePagesAsync(Guid roleId);
        Task<List<MenuModel>> getRolesPagesAsync(string userName);
    }
}
