using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brief.Models;
using Brief.Areas.Identity;

namespace Brief.Controllers
{
    public class BlogController : Controller
    {
        [BindProperty]
        public Blog Input { get; set; }

        public ActionResult Index()
        {
            var blog = new Blog() { Id = 1, Title = "First Blog!", Content = "And though quaint purple once chamber bird store off be remember other a me whispered minute and rustling as bird" };
            return View(blog);
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
