using Brief.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        //private readonly ILogger<HomeController> _logger;
        private IConfiguration Configuration;

        public HomeController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        /*
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        */

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blogs()
        {
            //string connectionString = System.Configuration.ConfigurationManager.AppSettings["BriefContextConnection"];
            //string connectionString = ConfigurationManager.ConnectionStrings["BriefContextConnection"].ConnectionString;
            string connectionString = this.Configuration.GetConnectionString("BriefContextConnection");
            //string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Brief;Integrated Security=True;MultipleActiveResultSets=True";
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
