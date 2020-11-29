using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brief.Models;

namespace Brief.Controllers
{
    public class BlogController : Controller
    {
        
        public ActionResult DisplayBlog(int id)
        {
            var blog = new Blog() { Creator = "Justin", Title = "First Blog!", Id = 15 };
            return View(blog);
        }
    }
}
