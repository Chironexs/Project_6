using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RoomBooking.MVC.Initial
{
    public static class DbInitializer
    {
        public static async void CreateRole(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }


        public static async void CreateFirstUser(UserManager<IdentityUser> userManager)
        {
            //A second operation started on this context before a previous operation completed.
            //This is usually caused by different threads using the same instance of DbContext.
            //For more information on how to avoid threading issues with DbContext, 
            if (await userManager.FindByEmailAsync("user@user.pl") == null)
            {
                IdentityUser identityUser = new IdentityUser
                {
                    UserName = "user@user.pl",
                    Email = "user@user.pl",
                };
                await userManager.CreateAsync(identityUser, "User2020!");
                await userManager.AddToRoleAsync(identityUser, "User");
            }
        }

        public static async void CreateFirstAdmin(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@admin.pl") == null)
            {
                IdentityUser identityUser = new IdentityUser
                {
                    UserName = "admin@admin.pl",
                    Email = "admin@admin.pl",
                };
                await userManager.CreateAsync(identityUser, "Admin2020!");
                await userManager.AddToRoleAsync(identityUser, "Admin");
            }
        }
    }
}