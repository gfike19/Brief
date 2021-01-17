using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Brief.Areas.Identity.Data;
using Brief.Data;
using Brief.Models;
using Brief.Models.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using static Brief.Models.Blog;

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
        public async Task<IActionResult> Password()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Password(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            model.StatusMessage = "Your password has been changed.";
            return View(model);
        }

        private async Task<EmailModel> LoadAsync(BriefUser user, string status = "")
        {

            EmailModel userInfo = new EmailModel
            {
                Email = await _userManager.GetEmailAsync(user),
                NewEmail = "",
                StatusMessage = status,
                IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user)
            };

            return userInfo;
        }

        [HttpGet]
        public async Task<IActionResult> Email()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            EmailModel model = await LoadAsync(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Email(EmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string statusMessage;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (model.NewEmail != email && model.NewEmail == model.ConfirmEmail)
            {
                user.Email = model.NewEmail;
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
                var result = await _userManager.ChangeEmailAsync(user, model.NewEmail, code);
                if (result.Succeeded)
                {
                    await _userManager.UpdateAsync(user);
                    statusMessage = "Your Email was updated successfully.";
                    model = await LoadAsync(user, statusMessage);
                    return View(model);
                }
            }

            statusMessage = "Error. Your email was not updated.";
            model = await LoadAsync(user, statusMessage);
            return View(model);
        }

        public async Task<IActionResult> History(int pageNumber = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(await UserBlogList<Blog>.CreateAsync(_context.Blogs.Where(m => m.CreatorId == user.Id).OrderByDescending(a => a.TimeCreated), pageNumber, 25));
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

            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            var delBlog = await _context.DeletedBlogs.FindAsync(id);
            delBlog.DeletedBy = user.Id;
            delBlog.PostStatus = "Deleted";
            _context.SaveChanges();

            return RedirectToAction("History", "Manage");
        }

        public async Task<ActionResult> Edit(string blogID)
        {

            if (blogID == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Blog model = _context.Blogs.Single(m => m.BlogID == blogID);

            if (user.Id == model.CreatorId)
            {

                List<BlogTag> BlogTagList = await BlogTagList<BlogTag>.CreateAsync(_context.BlogTags.Where(m => m.BlogID == model.BlogID));
                model.TagList = new List<BTag>();

                for (int i = 0; i <= BlogTagList.Count - 1; i++)
                {
                    Tag tag = _context.Tags.Single(m => m.TagID == BlogTagList[i].TagID);
                    model.TagList.Add(new BTag { TagName = tag.Title });
                }

                return View(model);
            }

            return new EmptyResult();
        }

        [HttpPost]
        public IActionResult UpdateDetails(string blogID)
        {

            if (blogID == null)
            {
                return NotFound();
            }

            Blog model = _context.Blogs.Single(m => m.BlogID == blogID);

            model.Title = HttpContext.Request.Form["txtTitle"].ToString();
            model.Content = HttpContext.Request.Form["txtContent"].ToString();
            model.TagString = HttpContext.Request.Form["tagString"].ToString();
            model.RemoveString = HttpContext.Request.Form["removeString"].ToString();
            _context.SaveChanges();

            if (model.RemoveString != "")
            {
                RemoveTags(model.RemoveString, model.BlogID);
            }

            if (model.TagString != "")
            {
                BlogController update = new BlogController(_context, _userManager);
                update.CreateTag(model.TagString);
                update.PairTags(model.TagString, model.BlogID);
            }

            return RedirectToAction("Index", "Blog", new {blogID = model.BlogID});
        }

        public void RemoveTags(string removeString, string blogID)
        {
            List<string> tagNames = removeString.Split(',').ToList();
            List<string> tagIDs = new List<string>();
            string SQLQuery;

            for (int i = 0; i <= tagNames.Count - 1; i++)
            {
                var tag = _context.Tags.Single(m => m.Title == tagNames[i]);
                tagIDs.Add(tag.TagID);
            }

            using (SqlConnection con = new SqlConnection(GetConString.ConString()))
            using (SqlCommand cmd = con.CreateCommand())
            {
                
                con.Open();

                for (int i = 0; i < tagNames.Count; i++)
                {
                    SQLQuery = "DELETE FROM BlogTags WHERE TagID = @tagID" + i + " AND BlogID = @blogID" + i;
                    cmd.CommandText = SQLQuery;
                    cmd.Parameters.AddWithValue("@tagID" + i, tagIDs[i]);
                    cmd.Parameters.AddWithValue("@blogID" + i, blogID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
