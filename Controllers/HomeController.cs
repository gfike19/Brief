using Brief.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
//using Brief.Data;

namespace Brief.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {/*
            using (var context = new Context())
            {
                context.Blogs.Add(new Blog()
                {
                    Title = "Test Post",
                    Content = "Blog Content will go here."
                }) ;
                context.SaveChanges();
                context.Dispose();
            }
            */
            return View();
        }

        public IActionResult Blogs()
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["Context"].ToString();
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Brief;Integrated Security=True;MultipleActiveResultSets=True";
            string sql = "SELECT * FROM dbo.Blogs";
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand(sql, conn);

            var model = new List<Brief.Models.Blog>();
            using (conn)
            {
                conn.Open();
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
            return View(model);
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
