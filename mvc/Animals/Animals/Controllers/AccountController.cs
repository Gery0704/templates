using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Animals.Controllers
{
    public class AccountController : BaseController
    {
        private readonly MyDbContext _context;

        private readonly UserManager<MyUsers> _userManager;
        private readonly SignInManager<MyUsers> _signInManager;

        //private readonly ITransactionService _service;

        public AccountController(UserManager<MyUsers> userManager, SignInManager<MyUsers> signInManager, MyDbContext context, ApplicationState applicationState)
            : base (applicationState)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        /// Bejelentkezés.
        /// </summary>
        /// <param name="user">A bejelentkezés adatai.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Login", user);

            // bejelentkeztetjük a felhasználót
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.UserPassword, user.RememberLogin, false);
            if (!result.Succeeded)
            {
                // nem szeretnénk, ha a felhasználó tudná, hogy a felhasználónévvel, vagy a jelszóval van-e baj, így csak általános hibát jelzünk
                ModelState.AddModelError("", "Hibás felhasználónév, vagy jelszó.");
                return View("Login", user);
            }

            // ha sikeres volt az ellenőrzés


            // ha sikeres volt az ellenőrzés, akkor a SignInManager már beállította a munkamenetet      
            _applicationState.UserCount++; // módosítjuk a felhasználók számát
            return RedirectToAction("Index", "Home"); // átirányítjuk a főoldalra
        }
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home"); // átirányítjuk a főoldalra
        }
    }
}