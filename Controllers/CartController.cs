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

    public IActionResult Index() 
    {
        return View(service.FindCart(_manager.GetUserAsync(HttpContext.User)).Products); 
    }

    public IActionResult BuyAll()
    {
        Cart cart = service.FindCart(_manager.GetUserAsync(HttpContext.User));
        cart.RemoveProducts(); 
        
        _context.Carts.Update(cart); 
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}