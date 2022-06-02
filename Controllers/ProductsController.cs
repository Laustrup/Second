using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Entities;
using Data;

namespace Controllers 
{
    [Authorize]
    public class ProductsController : Controller 
    {
        
        private WebshopContext _context;
        private readonly UserManager<IdentityUser> _manager;
        
        public ProductsController(WebshopContext context, UserManager<IdentityUser> manager) 
        {
            _context = context;
            _manager = manager;
        }

        [AllowAnonymous]
        public IActionResult Index() { return View( _context.Products.ToList()); }

        public IActionResult Create() { return View(); }

        [HttpPost]
        public IActionResult Create([Bind("Title", "Description", "Price", "Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.User = _manager.GetUserAsync(HttpContext.User).Result;
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id) { return View(_context.Products.Find(id)); }

        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id", "Title", "Description", "Price", "Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult AddToCart(int id, [Bind("Id", "Title", "Description", "Price", "Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                Cart cart = _context.Carts.Find(id);
                cart.AddProduct(product);
                _context.Carts.Update(cart);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}