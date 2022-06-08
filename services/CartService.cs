using Data;
using Entities;
using Entities.Statuses;
using Microsoft.AspNetCore.Identity;

namespace services
{
    public class CartService
    {
        private WebshopContext _context;

        public CartService(WebshopContext context) { _context = context; }

        public async Task<Cart> FindCart(Task<IdentityUser> user)
        {
            List<Cart> carts = _context.Carts.ToList();
            
            foreach (var cart in carts) 
            {
                if (cart.UserId != null && cart.UserId == user.Id.ToString()) { return cart; }
            } 
            return await GenerateCart(user.Result);
        }

        public async Task<Cart> GenerateCart(IdentityUser user)
        {
            Cart cart = new Cart();
            
            cart.User = user;
            cart.UserId = user.Id;
            cart.Products = new List<Product>();

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return cart;
        }
        
        public Cart BuyProducts(Cart cart)
        {
            List<Product> products = cart.Products;

            foreach (var product in products)
            {
                product.Status = ProductStatus.SOLD;
                products.Remove(product);
            }

            return cart;
        }
    }
}


