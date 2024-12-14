using Microsoft.EntityFrameworkCore;
using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Logic.Manager
{
    public class CategoryManager:BaseManager<Category,OdaiDbContext>
    {
        private readonly OdaiDbContext _context;
        public CategoryManager(OdaiDbContext context):base(context)
        {
            _context = context;
        }

        public override async Task<Category> GetById(int id)
        {
            return await _context.Category.Include(c => c.Products).FirstOrDefaultAsync(p => p.Id == id); ;
        }
    }
}
