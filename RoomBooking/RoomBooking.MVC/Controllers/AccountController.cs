using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.MVC.Models.DbModels;
using RoomBooking.MVC.Models.ViewModels;
using System.Threading.Tasks;

namespace RoomBooking.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(SignInManager<IdentityUser> _signInManager,
            UserManager<IdentityUser> _userManager,
            RoleManager<IdentityRole> _roleManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User()
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Address = registerViewModel.Address,
                    ZipCode = registerViewModel.ZipCode,
                    PhoneNumber = registerViewModel.PhoneNumber
                };
                var result = await userManager.CreateAsync(newUser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    var login = await signInManager.PasswordSignInAsync(registerViewModel.Email,registerViewModel.Password, true, false);

                    if (login.Succeeded)
                    {
                        await userManager.AddToRoleAsync(await userManager.FindByNameAsync(registerViewModel.Email), "User");

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nie można się zalogować!");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Add", "Booking");
                }
            }

            ModelState.AddModelError("", "Nie można się zalogować!");
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}