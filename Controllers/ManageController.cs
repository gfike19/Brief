using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Brief.Areas.Identity.Data;
using Brief.Data;
using Brief.Models;
using Brief.Models.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Brief.Controllers
{
    public partial class ManageController : Controller
    {
        private readonly BriefContext _context;
        private readonly UserManager<BriefUser> _userManager;
        private readonly SignInManager<BriefUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public ManageController(BriefContext context, UserManager<BriefUser> userManager, SignInManager<BriefUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            IndexModel userInfo = new IndexModel
            {
                FirstName = _userManager.GetUserAsync(User).Result.FirstName.ToString(),
                LastName = _userManager.GetUserAsync(User).Result.LastName.ToString()
            };

            return View(userInfo);
        }

        [HttpGet]
        public async Task<IActionResult> Email()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            EmailModel userInfo = new EmailModel
            {
                Email = await _userManager.GetEmailAsync(user),
                NewEmail = "",
                IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user)
            };

            return View(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //if (!ModelState.IsValid)
            //{
            //    await LoadAsync(user);
            //    return Page();
            //}

            EmailModel userInfo = new EmailModel
            {
                Email = await _userManager.GetEmailAsync(user),
                NewEmail = HttpContext.Request.Form["txtNewEmail"].ToString(),
                IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user)
            };

            //var email = await _userManager.GetEmailAsync(user);
            //var newEmail = HttpContext.Request.Form["txtNewEmail"].ToString();
            if (userInfo.NewEmail != userInfo.Email)
            {
                await _userManager.SetEmailAsync(user, userInfo.NewEmail);

                userInfo.StatusMessage = "Your Email was updated successfully.";
                return RedirectToAction(nameof(ManageController.Email), "Manage");
            }

            userInfo.StatusMessage = "Your email is unchanged.";
            return View("Email");
        }

        public async Task<IActionResult> History(int pageNumber = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await UserBlogList<Blog>.CreateAsync(_context.Blogs.Where(m => m.CreatorId == user.Id).OrderByDescending(a => a.TimeCreated), pageNumber, 25));
            //return View(await UserBlogList<Blog>.CreateAsync(_context.Blogs.OrderByDescending(a => a.TimeCreated), pageNumber, 15));
        }

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
            delBlog.PostStatus = "Deleted";
            _context.SaveChanges();

            return RedirectToAction("History", "Manage");
        }
    }
}
