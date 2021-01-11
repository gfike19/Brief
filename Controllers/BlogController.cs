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
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ViewBag.Result = "Something Went Wrong";
            }

            return View("Create");

        }

        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var blog = await _context.Blogs.FindAsync(id);

            if (user.Id == blog.CreatorId || User.IsInRole("Admin"))
            {
                
                _context.Blogs.Remove(blog);
                _context.SaveChanges();
                var delBlog = await _context.DeletedBlogs.FindAsync(id);
                delBlog.DeletedBy = user.Id;
                if (User.IsInRole("Admin"))
                {
                    delBlog.PostStatus = "Removed";
                }
                else
                {
                    delBlog.PostStatus = "Deleted";
                }
                _context.SaveChanges();
                
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View("Index");
        }
    }
}
