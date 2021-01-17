using Brief.Areas.Identity.Data;
using Brief.Data;
using Brief.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.ViewModels
{
    public class AdminVM
    {
        public UserManager<BriefUser> userManager;
        public BriefContext context;
        public LoggedUserModel users;
        public UserBlogList<BriefUser> blogs;
    }
}
