using Brief.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brief
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<BriefUser> userManager, RoleManager<MyIdentityRole> roleManager)
        {
        }

        public static void SeedUsers(UserManager<BriefUser> userManager)
        {
        }

        public static void SeedRoles(RoleManager<BriefUser> roleManager)
        {
        }
    }
}
