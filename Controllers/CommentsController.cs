using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace Controllers
{
    public class CommentsController : Controller
    {
        private readonly WebshopContext _context;
        private readonly UserManager<IdentityUser> _manager;

        public CommentsController(WebshopContext context, UserManager<IdentityUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        public async Task<IActionResult> Index(int id)
        {
            Product product = await _context.Products.FindAsync(id);

            if (product.Comments == null)
            {
                product.Comments = new List<Comment>();

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            
            return View(product);
        }

        public async Task<IActionResult> Create(int id)
        {
            ViewData["ProductId"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id","Product","Content")] Comment comment) 
        {
            if (ModelState.IsValid)
            {
                comment.TimeStamp = DateTime.Now;
                comment.User = await _manager.GetUserAsync(HttpContext.User);
                comment.UserId = comment.User!.Id;
                
                _context.Add(comment);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id", "Product", "Content")] Comment comment)
        {
            if (id != comment.Id) { return NotFound(); }

            if (ModelState.IsValid)
            {
                try
                {
                    Comment commentFromDb = _context.Comments.Find(comment.Id);
                    commentFromDb.Content = comment.Content;

                    _context.Update(commentFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Comments.Any(comment => comment.Id == id)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(comment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            var comment = await _context.Comments
            .Include(comment => comment.Product)
            .FirstOrDefaultAsync(comment => comment.Id == id);

            if (comment == null) { return NotFound();}

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) 
        {
            _context.Comments.Remove((await _context.Comments.FindAsync(id))!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}