using ECommercePlatform.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Odai.DataModel;

namespace ECommercePlatform.Logic.Services.RoleMenuItems
{
    public class RoleMenuItemsService(OdaiDbContext _context, UserManager<Domain.Identity.User> userManager) : IRoleMenuItemsService
    {
        public async Task<List<RoleMenuItemsModel>> GetRolePagesAsync(Guid roleId)
        {
            var rolePagesIds = await _context.RoleMenuItems.Where(rp => rp.RoleId == roleId).Select(rp => rp.MenuItemId).ToListAsync();

            var allPages = await _context.Menu.ToListAsync();

            var pages = allPages
           .Where(p => p.ParentId == null)
           .OrderBy(p => p.Order)
           .Select(p => new RoleMenuItemsModel
           {
               PageId = p.Id,
               Order = p.Order,
               ParentId = p.ParentId,
               NameKey = p.NameKey,
               Level = p.Level,
               Icon = p.Icon,
               IsSelected = rolePagesIds.Contains(p.Id),
               Url = p.Url,
               SubItems = GetSubPages(allPages, rolePagesIds, p.Id)
           })
           .ToList();

            return pages;
        }
        private List<RoleMenuItemsModel> GetSubPages(List<Domain.AccessControl.Menu> allPages, List<int> rolePagesIds, int parentId)
        {
            return GetSubPagesRecursive(allPages, parentId).ToList();

            IEnumerable<RoleMenuItemsModel> GetSubPagesRecursive(List<Domain.AccessControl.Menu> pages, int pid)
            {
                return pages
                    .Where(p => p.ParentId == pid)
                    .OrderBy(p => p.Order)
                    .Select(p => new RoleMenuItemsModel
                    {
                        PageId = p.Id,
                        Order = p.Order,
                        ParentId = p.ParentId,
                        NameKey = p.NameKey,
                        Level = p.Level,
                        Icon = p.Icon,
                        Url = p.Url,
                        IsSelected = rolePagesIds.Contains(p.Id),
                        SubItems = GetSubPagesRecursive(pages, p.Id).ToList()
                    });
            }
        }

        public async Task<List<MenuModel>> getRolesPagesAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return new List<MenuModel>();
            }

            var roles = await userManager.GetRolesAsync(user);

            var rolesIds = await _context.Roles
                .Where(r => roles.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync();

            var rolePagesIds = await _context.RoleMenuItems
                .Where(rp => rolesIds.Contains(rp.RoleId))
                .Distinct().Select(rp => rp.MenuItemId)
                .ToListAsync();

            var allPages = await _context.Menu
                .Where(p => rolePagesIds.Contains(p.Id))
                .ToListAsync();

            var pages = allPages
                .Where(p => p.ParentId == null)
                .OrderBy(p => p.Order)
                .Select(p => new MenuModel
                {
                    Id = p.Id,
                    Order = p.Order,
                    ParentId = p.ParentId,
                    NameKey = p.NameKey,
                    Level = p.Level,
                    Icon = p.Icon,
                    Url = p.Url,
                    SubItems = GetSubPages(allPages, p.Id)
                })
                .ToList();

            return pages;
        }
        private List<MenuModel> GetSubPages(List<Domain.AccessControl.Menu> allPages, int parentId)
        {
            return GetSubPagesRecursive(allPages, parentId).ToList();

            IEnumerable<MenuModel> GetSubPagesRecursive(List<Domain.AccessControl.Menu> pages, int pid)
            {
                return pages
                    .Where(p => p.ParentId == pid)
                    .OrderBy(p => p.Order)
                    .Select(p => new MenuModel
                    {
                        Id = p.Id,
                        Order = p.Order,
                        ParentId = p.ParentId,
                        NameKey = p.NameKey,
                        Level = p.Level,
                        Icon = p.Icon,
                        Url = p.Url,
                        SubItems = GetSubPagesRecursive(pages, p.Id).ToList()
                    });
            }
        }

        public async Task<bool> ToggleRolePagesAsync(Guid roleId, List<int> pagesIds)
        {
            var existsRolePages = _context.RoleMenuItems.Where(rp => rp.RoleId == roleId).ToList();

            _context.RemoveRange(existsRolePages);

            var rolePages = pagesIds.Select(menuItemId => new Domain.AccessControl.RoleMenuItems
            {
                RoleId = roleId,
                MenuItemId = menuItemId
            });

            await _context.AddRangeAsync(rolePages);

            return await _context.SaveChangesAsync() > 0;
        }
       
      
    }
}
