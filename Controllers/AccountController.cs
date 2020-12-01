using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brief.Areas.Identity.Data;
using Brief.Models;
//using Brief.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Brief.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegistrationModel userModel)
        {
            return View();
        }

        private readonly IMapper _mapper;
        private readonly UserManager<BriefUser> _userManager;
        public AccountController(IMapper mapper, UserManager<BriefUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
    }
}
