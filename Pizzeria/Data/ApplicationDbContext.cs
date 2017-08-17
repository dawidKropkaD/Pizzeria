using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Models.Tables;
using Pizzeria.Models;

namespace Pizzeria.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductDb> ProductDb { get; set; }
        public DbSet<AdditionalComponent> AdditionaComponent { get; set; }
        public DbSet<OrderedProduct> OrderedProduct { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<UserDb> UserDb { get; set; }
        public DbSet<Promotion> Promotion { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
