using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Odai.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.DataModel
{
    public class OdaiDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
    {
        public OdaiDbContext(DbContextOptions<OdaiDbContext> options):base(options)
        {
            
        }
        //Add-Migration initialize -project "Odai.DataModel" 
        //update-database  -project "Odai.DataModel" 
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rating>Ratings { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> baskets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
