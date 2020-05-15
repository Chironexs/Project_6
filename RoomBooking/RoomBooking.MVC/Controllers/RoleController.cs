using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoomBooking.MVC.Models;
using Microsoft.AspNetCore.Identity;
using RoomBooking.MVC.Models.DbModels;

namespace RoomBooking.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(SignInManager<IdentityUser> _signInManager,
            UserManager<IdentityUser> _userManager,
            RoleManager<IdentityRole> _roleManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await userManager.FindByEmailAsync("jan.nowak@wp.pl") == null)
            {
                User user = new User
                {
                    Email = "jan.nowak@wp.pl",
                    UserName = "jan.nowak@wp.pl",
                    FirstName = "Jan",
                    LastName = "Nowak",
                    PhoneNumber = "607829546",
                    Address = "Złota 44, Warszawa",
                    ZipCode = "00-700",

                };
                await userManager.CreateAsync(user, "QAZzaq123@");
                await userManager.AddToRoleAsync(user, "User");

//                IdentityUser identityUser = new IdentityUser
//                {
//                    UserName = "user@user.pl",
//                    Email = "user@user.pl",
//                };
//                await userManager.CreateAsync(identityUser, "User2020!");
//                await userManager.AddToRoleAsync(identityUser, "User");
            }
            if (await userManager.FindByEmailAsync("karol@onet.pl") == null)
            {
                //                IdentityUser identityUser = new IdentityUser
                //                {
                //                    UserName = "admin@admin.pl",
                //                    Email = "admin@admin.pl",
                //                };

                User user = new User
                {
                    Email = "karol@onet.pl",
                    UserName = "karol@onet.pl",
                    FirstName = "Karol",
                    LastName = "Karolak",
                    PhoneNumber = "607485656",
                    Address = "Chmielna 4, Warszawa",
                    ZipCode = "00-500",

                };
                await userManager.CreateAsync(user, "QAZzaq123@!");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}