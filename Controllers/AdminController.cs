using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brief.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Brief.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private IConfiguration Configuration;


        public AdminController(IMapper mapper, IConfiguration _configuration, ILogger<AdminController> logger)
        {
            _mapper = mapper;
            Configuration = _configuration;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string connectionString = this.Configuration.GetConnectionString("BriefContextConnection");
            string sql = "DELETE FROM dbo.Blogs WHERE id=" + id.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandText = sql;
            //cmd.Parameters.AddWithValue("@id", id);
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return View();
        }

    }
}
