using Brief.Data;
using Brief.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.Controllers
{
    public class HomeController : Controller
    {
        private readonly BriefContext _context;

        public HomeController(BriefContext context)
        {
            _context = context;
            
        }

        public async Task<IActionResult> Index(int pageNumber=1, string sortBy="newest")
        {
            if (sortBy == "oldest")
            {
                return View(await PaginatedList<Blog>.CreateAsync(_context.Blogs.OrderBy(a => a.TimeCreated), pageNumber, 15));

            }
            return View(await PaginatedList<Blog>.CreateAsync(_context.Blogs.OrderByDescending(a => a.TimeCreated), pageNumber, 15));

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
