using Brief.Data;
using Brief.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.Security
{
    public class AppSignInManager : SignInManager<AppUser, string>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
        : base(userManager, authenticationManager)
        {
        }
    }
}
