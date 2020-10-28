using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bardcore.Models
{
    public class SetupSecurity
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {

            IdentityUser admin = userManager.FindByEmailAsync("admin@bardcore.com").Result;

            if (admin== null)
            {
                IdentityUser sysadmin = new IdentityUser();
                sysadmin.Email = "admin@bardcore.com";
                sysadmin.UserName = "admin@bardcore.com";

                IdentityResult result = userManager.CreateAsync(sysadmin, "B@rdLyf3").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(sysadmin, "Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "NormalUser";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
