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
            SeedUser(builder);
            SeedProducts(builder);
            SeedComments(builder);
        }

        public void SeedUser(ModelBuilder builder)
        {
            var user = new IdentityUser
            {
                Id = "1",
                Email = "laust.bonnesen@mail.com", EmailConfirmed = true,
                UserName = "Laustrup"
            };

            PasswordHasher<IdentityUser> passHash = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passHash.HashPassword(user,"123456!A");

            builder.Entity<IdentityUser>().HasData(user);
        }
        
        public DbSet<Product> Products { get; set; }
        private void SeedProducts(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product() 
                {
                    Id = 1, Title = "Gibson Les Paul Standard",
                    Description = "This is a guitar",
                    Price = 15000,
                    Status = ProductStatus.UNSOLD,
                    UserId = "1", UserEmail = "laust.bonnesen@mail.com"
                }
            );
        }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Comment> Comments { get; set; }
        
        public DbSet<CartIndex> CartIndices { get; set; }
        private void SeedComments(ModelBuilder builder)
        {
            builder.Entity<Comment>().HasData(
                new Comment()
                {
                    CommentId = 1, Content = "This is the first comment!",
                    ProductId = 1, UserId = "1",
                    TimeStamp = DateTime.Now
                }
            );
        }
    }
}