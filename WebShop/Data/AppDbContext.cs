using Microsoft.EntityFrameworkCore;
using WebShop.Models;
namespace WebShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price).HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(i => i.UnitPrice).HasPrecision(10, 2);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email).IsUnique();
        }

    }
}
