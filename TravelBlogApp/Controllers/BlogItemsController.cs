using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelBlogApp.Data;
using TravelBlogApp.Models;

namespace TravelBlogApp.Controllers
{
    public class BlogItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BlogItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogItems.Include(b => b.Category);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: BlogItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,IsPublished,CategoryId")] BlogItem blogItem)
        {
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", blogItem.CategoryId);
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", blogItem.CategoryId);
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

        private bool BlogItemExists(int id)
        {
            return _context.BlogItems.Any(e => e.Id == id);
        }
    }
}
