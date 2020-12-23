using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brief.Areas.Identity.Data;
using Brief.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Brief.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        public IServiceCollection services;
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

        [Authorize(Roles = "Admin")]
        private async Task<ActionResult> MakeAdmin(IServiceProvider serviceProvider, string Email)
        {
            var RoleManager = serviceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<AppRole>>();
            var UserManager = serviceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<BriefUser>>();

            Microsoft.AspNetCore.Identity.IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new AppRole("Admin"));
            }

            //var UserManager = serviceProvider.GetRequiredService<UserManager<BriefUser>>();
            BriefUser user = await UserManager.FindByEmailAsync(Email);
            await UserManager.AddToRoleAsync(user, "Admin");
            return null;
        }

    }
}
