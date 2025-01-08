using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entity;

namespace WebApp.Data
{
    public class WebAppDbContext : IdentityDbContext<User>
    {
        public WebAppDbContext(DbContextOptions options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<OrderHistory> OrderHistory { get; set; }
    }
}
