using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Brief.Areas.Identity.Data;
using Brief.Models.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Brief.Controllers
{
    public partial class ManageController : Controller
    {
        private readonly UserManager<BriefUser> _userManager;
        private readonly SignInManager<BriefUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public ManageController( UserManager<BriefUser> userManager, SignInManager<BriefUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, userInfo.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = userInfo.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    userInfo.NewEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                userInfo.StatusMessage = "Confirmation link to change email sent. Please check your email.";
                return View(userInfo);
            }

            userInfo.StatusMessage = "Your email is unchanged.";
            return View(userInfo);
        }
    }
}
