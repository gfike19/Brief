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
using Brief.Data;
using Brief.ViewModels;

namespace Brief.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        public IServiceCollection services;
        private IConfiguration Configuration;
        private readonly BriefContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<BriefUser> _userManager;


        public AdminController(IMapper mapper, IConfiguration _configuration, ILogger<AdminController> logger, BriefContext context, Microsoft.AspNetCore.Identity.UserManager<BriefUser> userManager)
        {
            _mapper = mapper;
            Configuration = _configuration;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            AdminVM adminModel = new AdminVM();
            //adminModel.context.BriefUsers.
            return View(adminModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecentlyPosted(int pageNumber = 1)
        {
            return View(await PaginatedList<Blog>.CreateAsync(_context.Blogs.OrderByDescending(a => a.TimeCreated), pageNumber, 15));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecentlyDeleted(int pageNumber = 1)
        {
            return View(await PaginatedList<DeletedBlog>.CreateAsync(_context.DeletedBlogs.OrderByDescending(a => a.TimeCreated), pageNumber, 15));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeAdmin(string email)
        {
            BriefUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var blog = await _context.Blogs.FindAsync(id);

            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            var delBlog = await _context.DeletedBlogs.FindAsync(id);
            delBlog.DeletedBy = user.Id;
            delBlog.PostStatus = "Removed";
            _context.SaveChanges();

            return RedirectToAction("RecentlyPosted", "Admin");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UndoDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var user = await _userManager.GetUserAsync(User);
            var delBlog = await _context.DeletedBlogs.FindAsync(id);

            _context.DeletedBlogs.Remove(delBlog);
            _context.SaveChanges();
            var blog = await _context.Blogs.FindAsync(id);
            blog.PostStatus = null;
            _context.SaveChanges();

            return RedirectToAction("RecentlyDeleted", "Admin");
        }
    }
}
