using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities;

namespace Controllers
{
    public class CommentsController : Controller
    {
        private readonly WebshopContext _context;

        public CommentsController(WebshopContext context) {_context = context;}

        public async Task<IActionResult> Index() { return View(await _context.Comments.Include(c => c.Product).ToListAsync()); }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }

            var comment = await _context.Comments.Include(c => c.Product).FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null) { return NotFound();}

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id","Author","Product","Content")] Comment comment) 
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id","Author","Product","Content")] Comment comment)
        {
            if (id != comment.Id) { return NotFound(); }

            if (ModelState.IsValid)
            {
                try 
                {
                    _context.Update(comment);
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
            _context.Comments.Remove(await _context.Comments.FindAsync(id));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}