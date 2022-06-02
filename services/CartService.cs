using Data;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace services
{
    public class CartService
    {
        private WebshopContext _context;

        public CartService(WebshopContext context) { _context = context; }

        public Cart FindCart(Task<IdentityUser> user)
        {
            List<Cart> carts = _context.Carts.ToList();
        
            foreach (var cart in carts)
            {
                if (cart.User != null)
                {
                    int cartId;
                    Int32.TryParse(cart.User.Id, out cartId);
                    
                    if (cartId == user.Id)
                    { 
                        return cart;
                    }
                }
            }
            return null;
        }
    }
}


