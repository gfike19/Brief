using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brief.Models;
using Brief.Areas.Identity;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Data;
using Brief.Data;

namespace Brief.Controllers
{
    public class BlogController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration Configuration;
        private readonly BriefContext _context;

        public BlogController(IMapper mapper, IConfiguration _configuration, ILogger<HomeController> logger, BriefContext context)
        {
            _mapper = mapper;
            Configuration = _configuration;
            _logger = logger;
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

        /*
        [HttpPost]
        public IActionResult Create(CreateBlog newBlog)
        {
            if (!ModelState.IsValid)
            {
                return View(newBlog);
            }
            var blog = _mapper.Map<Blog>(newBlog);
            blog.Title = newBlog.BlogTitle;

            
            string connectionString = this.Configuration.GetConnectionString("BriefContextConnection");
            string sql = "INSERT INTO Blogs (Title, Content) VALUES(@Title, @Content)";

            
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            //var model = new List<Brief.Models.Blog>();

            //cmd.CommandType = System.Data.CommandType.Text;
            //cmd.CommandText = "INSERT INTO Blogs (Title, Content) VALUES(@Title, @Content)";
            cmd.Parameters.AddWithValue("@Title", "Hard Coded Title");
            cmd.Parameters.AddWithValue("@Content", "Hard Coded Content");

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            /*
            using (var cn = new SqlClient.SqlConnection(yourConnectionString))
            using (var cmd = new SqlClient.SqlCommand())
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select * From Table Where Title = @Title";
                cmd.Parameters.Add("@Title", someone);
            }
            */
        /*
        SqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            var blog = new Blog();
            blog.CreatorName = rdr["CreatorName"].ToString();
            blog.Title = rdr["Title"].ToString();
            blog.Content = rdr["Content"].ToString();
            model.Add(blog);
        }
    }
    */
        //_mapper.Map<BriefUser>(userModel);
        /*
    var result = await 
    var result = await _userManager.CreateAsync(user, userModel.Password);
    if (!result.Succeeded)
    {
        foreach (var error in result.Errors)
        {
            ModelState.TryAddModelError(error.Code, error.Description);
        }
        return View(userModel);
    }
    await _userManager.AddToRoleAsync(user, "User");

        //return RedirectToAction(nameof(HomeController.Index), "Home");
        return View();
    */
    }
}
