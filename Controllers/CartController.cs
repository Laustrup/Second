using Microsoft.AspNetCore.Mvc;
using Data;
using Entities;
using Entities.Statuses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using services;

namespace Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private WebshopContext _context;
        private readonly UserManager<IdentityUser> _manager;

        private CartService service;

        public CartController(WebshopContext context, UserManager<IdentityUser> manager) 
        {
            _context = context;
            _manager = manager;
            service = new CartService(context);
        }
        
        public async Task<IActionResult> Index()
        {
            Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User));
            if (cart.Products!=null) {Console.WriteLine("\n\n" + cart.Products.Count + "\n\n");}
            
            return View(cart); 
        }
        
        public async Task<RedirectToActionResult> AddToCart(int id)
        {
            Product product = await _context.Products.FindAsync(id);

            if (product.Status != ProductStatus.SOLD)
            {
                Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User));
                cart.AddProduct(product);
                
                try
                {
                    CartIndex index = new CartIndex();
                    index.CartId = cart.Id;
                    index.ProductId = product.Id;
                    
                    await _context.CartIndices.AddAsync(index);
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch {Console.WriteLine("\nCouldn't add product\n\n");}
            }
            return RedirectToAction("Index");
        }
        
        public async Task<RedirectToActionResult> RemoveProduct(int id)
        {
            Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User));
            Product product = await _context.Products.FindAsync(id);
            var indices = from ci in _context.CartIndices select ci;
            indices.Where(cartIndex => cartIndex.ProductId == product.Id && cartIndex.CartId == cart.Id).ToList();

            cart.RemoveProduct(await _context.Products.FindAsync(id));
            _context.Update(cart);
            _context.CartIndices.Remove(indices.Where(index => index.ProductId == product.Id).ToList().First()); 

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> BuyAll()
        {
            _context.Carts.Update(await service.BuyProducts(service.FindCart(_manager.GetUserAsync(HttpContext.User)).Result));
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

