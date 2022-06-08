using Microsoft.AspNetCore.Mvc;
using Data;
using Entities;
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
            Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User));
            cart.AddProduct(await _context.Products.FindAsync(id));

            try
            {
                _context.Update(cart);
                await _context.SaveChangesAsync();
            }
            catch {Console.WriteLine("\nCouldn't add product\n\n");}

            return RedirectToAction("Index");
        }
        
        public async Task<RedirectToActionResult> RemoveProduct(int id)
        {
            Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User));
            cart.RemoveProducts();
            _context.Update(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> BuyAll()
        {
            _context.Carts.Update(service.BuyProducts(await service.FindCart(_manager.GetUserAsync(HttpContext.User)))); 
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

