using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelBlogApp.Data;
using TravelBlogApp.Models;

namespace TravelBlogApp.Controllers
{
    [Authorize]
    public class BlogItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Author> _userManager;

        public BlogItemsController(ApplicationDbContext context, UserManager<Author> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BlogItems
        public async Task<IActionResult> Index(SearchViewModel searchViewModel)
        {
            var author = await _userManager.GetUserAsync(HttpContext.User);
            var query = _context.BlogItems
                .Include(b => b.Category).Where(t=>t.AuthorId==author.Id).AsQueryable();
                
            if (!String.IsNullOrWhiteSpace(searchViewModel.SearchText))
            {
                query = query.Where(t => t.Title.Contains(searchViewModel.SearchText ));
            }
            
            query=query.OrderByDescending(t => t.CreatedDate);

            searchViewModel.Result = await query.ToListAsync();

            return View(searchViewModel);
        }

        // GET: BlogItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogItem = await _context.BlogItems
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogItem == null)
            {
                return NotFound();
            }

            return View(blogItem);
        }

        // GET: BlogItems/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategorySelectList = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: BlogItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,IsPublished,CategoryId")] BlogItem blogItem)
        {
            var author = await _userManager.GetUserAsync(HttpContext.User);

            blogItem.AuthorId = author.Id;

            if (ModelState.IsValid)
            {
                _context.Add(blogItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", blogItem.CategoryId);
            return View(blogItem);
        }

        // GET: BlogItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogItem = await _context.BlogItems.FindAsync(id);
            if (blogItem == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogItem.CategoryId);
            return View(blogItem);
        }

        // POST: BlogItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,IsPublished,CategoryId")] BlogItem blogItem)
        {
            if (id != blogItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogItemExists(blogItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogItem.CategoryId);
            return View(blogItem);
        }

        // GET: BlogItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogItem = await _context.BlogItems
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogItem == null)
            {
                return NotFound();
            }

            return View(blogItem);
        }

        // POST: BlogItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogItem = await _context.BlogItems.FindAsync(id);
            _context.BlogItems.Remove(blogItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        

        private async Task<ActionResult> ChangeStatus(int id,bool status)
        {
            var blogItemItem = _context.BlogItems.FirstOrDefault(t => t.Id == id);
            if (blogItemItem == null)
            {
                return NotFound();
            }
            blogItemItem.IsPublished = status;


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> Publish(int id)
        {
            return await ChangeStatus(id, true);
        }
        public async Task<ActionResult> UndoPublish(int id)
        {
            return await ChangeStatus(id, false);
        }

        private bool BlogItemExists(int id)
        {
            return _context.BlogItems.Any(e => e.Id == id);
        }
    }
}
