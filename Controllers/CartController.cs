using Microsoft.AspNetCore.Mvc;
using Data;
using Entities;
using Microsoft.AspNetCore.Identity;
using services;

namespace Controllers;

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
        return View(cart.Products); 
    }
    
    [HttpPost]
    public async Task<RedirectToActionResult> AddToCart(int id,
        [Bind("Id", "Title", "Description", "Price", "Status", "User", "UserId", "UserEmail")] Product product)
    {
        if (ModelState.IsValid)
        {
            Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User));
            cart.AddProduct(product);
            
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<RedirectToActionResult> RemoveProduct(int id, [Bind("Id", "Title", "Description", "Price", "Status")] Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<RedirectToActionResult> BuyAll()
    {
        Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User));
        cart.RemoveProducts(); 
        
        _context.Carts.Update(cart); 
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}