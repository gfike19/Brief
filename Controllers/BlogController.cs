using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Brief.Models;
using Brief.Data;
using Microsoft.AspNetCore.Identity;
using Brief.Areas.Identity.Data;
using System.Diagnostics;

namespace Brief.Controllers
{
    public class BlogController : Controller
    {
        private readonly BriefContext _context;
        private readonly UserManager<BriefUser> _userManager;

        public BlogController(BriefContext context, UserManager<BriefUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetBlogDetails()
        {
            Blog blogPost = new Blog
            {
                Title = HttpContext.Request.Form["txtTitle"].ToString(),
                Content = HttpContext.Request.Form["txtContent"].ToString(),
                CreatorId = _userManager.GetUserAsync(User).Result.Id.ToString(),
                CreatorName = _userManager.GetUserAsync(User).Result.FirstName + " " + _userManager.GetUserAsync(User).Result.LastName,
                TimeCreated = DateTime.Now
            };
            Debug.WriteLine(blogPost.Title);
            int result = blogPost.SaveDetails();
            if (result > 0)
            {
                ViewBag.Result = "Blog Posted Successfully";
            }
            else
            {
                ViewBag.Result = "Something Went Wrong";
            }
            return View("Create");
        }     
    }
}
