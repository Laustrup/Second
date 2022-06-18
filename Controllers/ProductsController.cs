using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Entities;
using Data;
using Entities.Statuses;

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

        /*
        [AllowAnonymous]
        public IActionResult Index() { return View( _context.Products.ToList()); }
        */
        
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString)
        {
            if (searchString != null)
            {
                var products = from p in _context.Products select p;
            
                if (!String.IsNullOrEmpty(searchString))
                {
                    products = products.Where(s => s.Title!.Contains(searchString));
                }
            
                return View(await products.ToListAsync());
            }

            return View(_context.Products.ToList());
        }

        public IActionResult Create() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title", "Description", "Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.User = await _manager.FindByNameAsync(HttpContext.User.Identity.Name);
                product.UserEmail = product.User.Email;
                product.Status = ProductStatus.UNSOLD;
                
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id) { return View(_context.Products.Find(id)); }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id", "Title", "Description", "Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.User = await _manager.GetUserAsync(HttpContext.User);
                product.UserId = product.User.Id;
                product.UserEmail = product.User.Email;
                product.Status = (await _context.Products.FindAsync(product.Id)).Status;
                
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = (await _context.Products.FindAsync(id));
            
            if (product!=null && ModelState.IsValid) 
            {
                _context.Remove(product);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }
    }
}