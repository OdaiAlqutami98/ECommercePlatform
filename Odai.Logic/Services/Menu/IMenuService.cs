using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Shared.Models;

namespace ECommercePlatform.Logic.Services.Menu
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuModel>> GetAllPagesAsync();
        Task<bool> AddPageAsync(MenuModel menu);
    }
}
