using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly WebshopContext _context;
        private readonly UserManager<IdentityUser> _manager;

        public CommentsController(WebshopContext context, UserManager<IdentityUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int id)
        {   
            Product product = await _context.Products.FindAsync(id);
            
            ViewData["ProductId"] = product.Id;
            ViewData["ProductTitle"] = product.Title;
            ViewData["ProductDescription"] = product.Description;

            var comments = from c in _context.Comments select c;

            comments = comments.Where(c => c.ProductId == id);
            
            return View(comments.ToList());
        }

        public IActionResult Create(int? id)
        {
            ViewData["ProductId"] = RouteData.Values["id"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId", "ProductId", "Content")] Comment comment) 
        {
            if (ModelState.IsValid)
            {
                comment.TimeStamp = DateTime.Now; 
                comment.User = await _manager.GetUserAsync(HttpContext.User); 
                comment.UserId = comment.User.Id;
                comment.UserEmail = comment.User.Email;
                comment.Product = await _context.Products.FindAsync(comment.ProductId);
                
                _context.Add(comment); 
                await _context.SaveChangesAsync();
                
                return RedirectToAction(
                    controllerName: "Comments",
                    actionName: "Index",
                    routeValues: new {id = comment.ProductId}
                );
            }
            return View();
        }
        
        public async Task<IActionResult> Edit(int id) {return View(await _context.Comments.FindAsync(id));}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId", "Content")] Comment comment)
        {
            if (id != comment.CommentId) { return NotFound(); }

            if (ModelState.IsValid)
            {
                Comment commentFromDb = await _context.Comments.FindAsync(id);
                try
                {
                    commentFromDb.Content = comment.Content;
                    _context.Comments.Update(commentFromDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("\nTrouble editing comment\n\n");
                    if (_context.Comments.Any(comment => comment.CommentId == id)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction( 
                    controllerName: "Comments",
                    actionName: "Index",
                    routeValues: new {id = commentFromDb.ProductId}
                );
            }

            return View(comment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            var comment = await _context.Comments
            .Include(comment => comment.Product)
            .FirstOrDefaultAsync(comment => comment.CommentId == id);

            if (comment == null) { return NotFound();}

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int productId = (await _context.Comments.FindAsync(id)).ProductId;
            _context.Comments.Remove((await _context.Comments.FindAsync(id))!);
            await _context.SaveChangesAsync();
            
            return RedirectToAction( 
                controllerName: "Comments",
                actionName: "Index",
                routeValues: new {id = productId}
            );
        }
    }
}