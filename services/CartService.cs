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
                if (cart.UserId != null && cart.UserId == user.Result.Id)
                {
                    var indices = from ci in _context.CartIndices select ci;
                    indices.Where(cartIndex => cartIndex.CartId == cart.Id).ToList();

                    List<Product> products = new List<Product>();
                    foreach (var index in indices)
                    {
                        products.Add(await _context.Products.FindAsync(index.ProductId));
                    }

                    cart.Products = products;
                    return cart;
                }
            } 
            Console.WriteLine("\n\nGenerates cart\n\n");
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
        
        public async Task<Cart> BuyProducts(Cart cart)
        {
            var indices = from ci in _context.CartIndices select ci;
            indices.Where(cartIndex => cartIndex.CartId == cart.Id).ToList();
            
            List<Product> products = new List<Product>();
            foreach (var index in indices)
            {
                products.Add(await _context.Products.FindAsync(index.ProductId));

                _context.CartIndices.Remove(index);
            }
            
            foreach (var product in products)
            {
                product.Status = ProductStatus.SOLD;
                 
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }

            cart.Products = null;

            return cart;
        }
    }
}
