using ECommercePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Odai.DataModel;

namespace ECommercePlatform.Logic.Services.Menu
{
    public class MenuService(OdaiDbContext _context) : IMenuService
    {
        public async Task<bool> AddPageAsync(MenuModel menu)
        {
            if (string.IsNullOrWhiteSpace(menu.NameKey))
            {
                return false;
            }
            var page = new Domain.AccessControl.Menu
            {
                NameKey = menu.NameKey,
                Icon = menu.Icon,
                Url = menu.Url,
                ParentId = menu.ParentId,
                Level = menu.Level,
                Order = menu.Order
            };
            await _context.AddAsync(page);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<MenuModel>> GetAllPagesAsync()
        {
            var allPages = await _context.Menu.ToListAsync();
            var pages = allPages.Where(p => p.ParentId == null).OrderBy(p => p.Order).Select(p => new MenuModel
            {
                Id = p.Id,
                ParentId = p.ParentId,
                Order = p.Order,
                Level = p.Level,
                Icon = p.Icon,
                Url = p.Url,
                NameKey = p.NameKey,
                SubItems = GetSubPages(allPages, p.Id)
            });
            return pages;
        }

        private List<MenuModel>GetSubPages(List<Domain.AccessControl.Menu> allPages,int parentId)
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
    }
}
