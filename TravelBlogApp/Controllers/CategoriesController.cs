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
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Author> _userManager;

        public CategoriesController(ApplicationDbContext context, UserManager<Author> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categories
        public async Task<IActionResult> Index(SearchViewModel searchViewModel)
        {
            var author = await _userManager.GetUserAsync(HttpContext.User);
            var query = _context.Categories.Where(t => t.AuthorId == author.Id).AsQueryable();
            if (!String.IsNullOrWhiteSpace(searchViewModel.SearchText))
            {
                query = query.Where(t => t.Name.Contains(searchViewModel.SearchText));
            }

            searchViewModel.Result2 = await query.ToListAsync();

            return View(searchViewModel);
           
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            var author = await _userManager.GetUserAsync(HttpContext.User);
            if (category.AuthorId != author.Id)
            {
                return Unauthorized();
            }
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            var author = await _userManager.GetUserAsync(HttpContext.User);

            category.AuthorId = author.Id;
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            var author = await _userManager.GetUserAsync(HttpContext.User);
            if (category.AuthorId != author.Id)
            {
                return Unauthorized();
            }
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldcategory = await _context.Categories.FindAsync(id);
                    var author = await _userManager.GetUserAsync(HttpContext.User);
                    if (oldcategory.AuthorId != author.Id)
                    {
                        return Unauthorized();
                    }
                    oldcategory.Name = category.Name;
                    oldcategory.Description = category.Description;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            var author = await _userManager.GetUserAsync(HttpContext.User);
            if (category.AuthorId != author.Id)
            {
                return Unauthorized();
            }
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
