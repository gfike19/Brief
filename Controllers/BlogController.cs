using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Brief.Models;
using Brief.Data;
using Microsoft.AspNetCore.Identity;
using Brief.Areas.Identity.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using static Brief.Models.Blog;

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
        public IActionResult Index(string blogID)
        {
            if (blogID == null)
            {
                return NotFound();
            }

            Blog model = _context.Blogs.Single(m => m.BlogID == blogID);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                Blog model = new Blog { };
                return View(model);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");

        }

        [HttpPost]
        public IActionResult Create(Blog model)
        {
            model.Title = HttpContext.Request.Form["txtTitle"].ToString();
            model.Content = HttpContext.Request.Form["txtContent"].ToString();

            return View(model);
        }

        [HttpPost]
        public IActionResult GetBlogDetails()
        {
            Blog model = new Blog
            {
                Title = HttpContext.Request.Form["txtTitle"].ToString(),
                Content = HttpContext.Request.Form["txtContent"].ToString(),
                CreatorId = _userManager.GetUserAsync(User).Result.Id.ToString(),
                CreatorName = _userManager.GetUserAsync(User).Result.FirstName + " " + _userManager.GetUserAsync(User).Result.LastName,
                TimeCreated = DateTime.Now,
                BlogID = Guid.NewGuid().ToString(),
                TagString = HttpContext.Request.Form["tagString"].ToString()
            };

            int result = model.SaveDetails();

            if (result > 0)
            {
                if (model.TagString != "")
                {
                    CreateTag(model.TagString);
                    PairTags(model.TagString, model.BlogID);
                }
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
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

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

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public void PairTags(string tagNameString, string blogID)
        {
            List<string> tagNames = tagNameString.Split(',').ToList();
            List<string> tagIDs = new List<string>();
            string paramString;
            string SQLQuery;

            tagNames = tagNames.ConvertAll(d => d.Substring(0, 1).ToUpper() + d.Substring(1));

            for (int i = 0; i <= tagNames.Count-1; i++)
            {
                var tag = _context.Tags.Single(m => m.Title == tagNames[i]);
                tagIDs.Add(tag.TagID);
            }

            if (tagNames.Count == 1)
            {
                paramString = "(@tagID1, @BlogID1)";
            }
            else
            {
                paramString = "(@tagID1, @BlogID1)";
                for (int i = 2; i <= tagNames.Count; i++)
                {
                    paramString = paramString + ", (@TagID" + i + ", @BlogID" + i + ")";
                }
            }

            SQLQuery = "INSERT INTO BlogTags(TagID, BlogID) VALUES " + paramString;

            using (SqlConnection con = new SqlConnection(GetConString.ConString()))
            using (SqlCommand cmd = con.CreateCommand())
            {

                cmd.CommandText = SQLQuery;

                for (int i = 1; i <= tagNames.Count; i++)
                {;
                    cmd.Parameters.AddWithValue("@TagID" + i, tagIDs[i-1]);
                    cmd.Parameters.AddWithValue("@BlogID" + i, blogID);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void CreateTag(string tagNameString)
        {
            List<string> tagNames = tagNameString.Split(',').ToList();
            string paramString;
            string SQLQuery;
            string id;

            tagNames = tagNames.ConvertAll(d => d.Substring(0, 1).ToUpper() + d.Substring(1));

            for (int i = tagNames.Count - 1; i >= 0; i--)
            {
                if (_context.Tags.Any(m => m.Title == tagNames[i]))
                {
                    tagNames.Remove(tagNames[i]);
                };
            }

            if (tagNames.Count == 0)
            {
                return;
            }

            else if (tagNames.Count == 1)
            {
                paramString = "(@tagID1, @Title1)";
            }
            else
            {
                paramString = "(@tagID1, @Title1)";
                for (int i = 2; i <= tagNames.Count; i++)
                {
                    paramString = paramString + ", (@TagID" + i + ", @Title" + i + ")";
                }
            }

            SQLQuery = "INSERT INTO Tags(TagID, Title) VALUES " + paramString;

            using (SqlConnection con = new SqlConnection(GetConString.ConString()))
            using (SqlCommand cmd = con.CreateCommand())
            {

                cmd.CommandText = SQLQuery;

                for (int i = 1; i <= tagNames.Count; i++)
                {
                    id = Guid.NewGuid().ToString();
                    cmd.Parameters.AddWithValue("@TagID" + i, id);
                    cmd.Parameters.AddWithValue("@Title" + i, tagNames[i - 1]);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
