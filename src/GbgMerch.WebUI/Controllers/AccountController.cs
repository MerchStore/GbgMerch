using Microsoft.AspNetCore.Mvc;
namespace GbgMerch.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Här skulle du normalt validera användarnamn och lösenord mot en databas
            if (username == "admin" && password == "pass")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                // Skapa en autentiseringstoken eller session här
                return RedirectToAction("Index", "Admin");
            }
            ModelState.AddModelError("", "Ogiltigt användarnamn eller lösenord.");
            return View();
        }
    }
}