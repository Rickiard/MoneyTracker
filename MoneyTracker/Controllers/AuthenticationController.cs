using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Models;
using MoneyTracker.Services;
using System.Security.Claims;

namespace MoneyTracker.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly Services.IAuthenticationService _authenticationService;

        public AuthenticationController(Services.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var loggedUser = await _authenticationService.Login(user);

            if (loggedUser == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(user);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loggedUser.Username),
                new Claim(ClaimTypes.NameIdentifier, loggedUser.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Dashboard", "Transactions");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var success = await _authenticationService.Register(user);

            if (!success)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View(user);
            }

            return RedirectToAction("Login", "Authentication");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Authentication");
        }
    }
}
