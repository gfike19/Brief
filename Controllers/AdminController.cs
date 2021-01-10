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

    }
}
