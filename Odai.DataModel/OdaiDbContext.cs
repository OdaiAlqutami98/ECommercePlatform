using ECommercePlatform.Domain.AccessControl;
using ECommercePlatform.Domain.Entities;
using ECommercePlatform.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Odai.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.DataModel
{
    public class OdaiDbContext : IdentityDbContext<User, Role, Guid>
    {
        public OdaiDbContext(DbContextOptions<OdaiDbContext> options):base(options)
        {
            
        }
        //Add-Migration initialize -project "ECommercePlatform.DataModel" 
        //update-database  -project "ECommercePlatform.DataModel" 
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<Rating>Ratings { get; set; }
        //public DbSet<BasketItem> BasketItems { get; set; }
        //public DbSet<Basket> Baskets { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        //public DbSet<Clients> Clients { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<RoleMenuItems> RoleMenuItems { get; set; }
        public DbSet<Settings> Settings { get; set; }
        //public DbSet<Year> Years { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
