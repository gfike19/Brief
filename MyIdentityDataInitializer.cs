using Brief.Areas.Identity.Data;
using Brief.Models;
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
            if (userManager.FindByNameAsync("user1").Result == null)
            {
                BriefUser user = new BriefUser();
                user.UserName = "user1";
                user.Email = "user1@localhost";
                user.FullName = "Nancy Davolio";
                user.BirthDate = new DateTime(1960, 1, 1);

                IdentityResult result = userManager.CreateAsync(user, "password_goes_here").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                }
            }


            if (userManager.FindByNameAsync("user2").Result == null)
            {
                BriefUser user = new BriefUser();
                user.UserName = "user2";
                user.Email = "user2@localhost";
                user.FullName = "Mark Smith";
                user.BirthDate = new DateTime(1965, 1, 1);

                IdentityResult result = userManager.CreateAsync(user, "password_goes_here").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<AppRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                AppRole role = new AppRole();
                role.Name = "NormalUser";
                role.Description = "Perform normal operations.";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                AppRole role = new AppRole();
                role.Name = "Administrator";
                role.Description = "Perform all the operations.";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
