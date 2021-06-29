using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TravelBlogApp.Data;
using TravelBlogApp.Models;

namespace TravelBlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index(SearchViewModel searchViewModel)
        {
           var query= dbContext.BlogItems
                .Include(t=>t.Category)
                .Where(b => b.IsPublished == true).AsQueryable();

            if (!String.IsNullOrWhiteSpace(searchViewModel.SearchText))
            {
                query = query.Where(t => t.Title.Contains(searchViewModel.SearchText));
            }


            query = query.OrderByDescending(t => t.CreatedDate).Take(10); ;

            searchViewModel.Result = await query.ToListAsync();
           
            return View(searchViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
