using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Brief.Models;
using Brief.Data;

namespace Brief.Controllers
{
    public class BlogController : Controller
    {
        private readonly BriefContext _context;

        public BlogController(BriefContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Blog Input { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Blogs.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetBlogDetails()
        {
            Blog umodel = new Blog
            {
                Title = HttpContext.Request.Form["txtTitle"].ToString(),
                Content = HttpContext.Request.Form["txtContent"].ToString(),
                CreatorName = User.Identity.Name.ToString(),
                TimeCreated = DateTime.Now
            };
            int result = umodel.SaveDetails();
            if (result > 0)
            {
                ViewBag.Result = "Data Saved Successfully";
            }
            else
            {
                ViewBag.Result = "Something Went Wrong";
            }
            return View("Create");
        }
    }
}
