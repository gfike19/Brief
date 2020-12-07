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

        public ActionResult Index()
        {
            var blog = new Blog() { Id = 1, Title = "First Blog!", Content = "And though quaint purple once chamber bird store off be remember other a me whispered minute and rustling as bird" };
            return View(blog);
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}
