using Microsoft.AspNetCore.Mvc;

namespace AutoServiceManager.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var normalizedEmail = email?.Trim().ToLowerInvariant();

            if (password == "password" && normalizedEmail == "testare@gmail.com")
            {
                HttpContext.Session.SetString("UserName", "Alexei Ciobanu");
                HttpContext.Session.SetString("UserInitials", "AC");
                HttpContext.Session.SetString("UserRole", "ADMINISTRATOR");
                return RedirectToAction("Index", "Home");
            }

            if (password == "password" && normalizedEmail == "user@gmail.com")
            {
                HttpContext.Session.SetString("UserName", "Utilizator");
                HttpContext.Session.SetString("UserInitials", "US");
                HttpContext.Session.SetString("UserRole", "USER");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email sau parolă incorectă.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
