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
            return View(await service.FindCart(_manager.GetUserAsync(HttpContext.User))); 
        }
        
        public async Task<RedirectToActionResult> AddToCart(int id)
        {
            Console.WriteLine(id + " Entered add to cart");
            Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User)); 
            Console.WriteLine(cart.Id + " cart found");
            cart.AddProduct(await _context.Products.FindAsync(id));
            Console.WriteLine(cart.Id + " added product");

            if (_context.Carts.Contains(cart)) { _context.Update(cart); }
            else { _context.Carts.Add(cart); }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        
        public async Task<RedirectToActionResult> RemoveProduct(int id)
        {
            _context.Products.Remove(await _context.Products.FindAsync(id));
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

