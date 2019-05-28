using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBank.Szervic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EBank.Szervic.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<Clients> _userManager;
        private readonly SignInManager<Clients> _signInManager;
        private readonly EBankContext _context;

        public AccountController(
            UserManager<Clients> userManager,
            SignInManager<Clients> signInManager,
            EBankContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet("login/{userName}/{userPassword}")]
        public async Task<IActionResult> Login(String userName, String userPassword)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userName, userPassword, false, false);
                var tmp = result;
                if (!result.Succeeded)
                    return Forbid();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}