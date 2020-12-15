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

namespace Brief.Controllers
{
    public class BlogController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration Configuration;
        public BlogController(IMapper mapper, IConfiguration _configuration, ILogger<HomeController> logger)
        {
            _mapper = mapper;
            Configuration = _configuration;
            _logger = logger;
        }

        public ActionResult Index()
        {
            var blog = new Blog() { Id = 1, Title = "First Blog!", Content = "And though quaint purple once chamber bird store off be remember other a me whispered minute and rustling as bird" };
            return View(blog);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

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
            */
            //return RedirectToAction(nameof(HomeController.Index), "Home");
            return View();
        }
    }
}
