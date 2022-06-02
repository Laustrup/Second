using Entities;
using Entities.Statuses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Data 
{
    public class WebshopContext : IdentityDbContext
    {
        public WebshopContext(DbContextOptions<WebshopContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedProducts(builder);
        }

        public DbSet<Product> Products { get; set; }

        private void SeedProducts(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product() 
                {
                    Id = 1, Title = "Gibson Les Paul Standard", Description = "This is a guitar", Price = 15000, Status = ProductStatus.UNSOLD
                }
            );
        }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}