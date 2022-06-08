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
        return View(cart); 
    }
    
    [HttpPost]
    public async Task<RedirectToActionResult> AddToCart(int id)
    {
        Cart cart = await service.FindCart(_manager.GetUserAsync(HttpContext.User)); 
        cart.AddProduct(await _context.Products.FindAsync(id));
        
        _context.Carts.Update(cart); 
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
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