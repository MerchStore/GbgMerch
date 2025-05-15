using Microsoft.AspNetCore.Mvc;
namespace GbgMerch.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            // Tar bort admin-sessionen
            HttpContext.Session.Remove("IsAdmin");

            // Skickar tillbaka till startsidan
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Här skulle du normalt validera användarnamn och lösenord mot en databas
            if (username == "admin" && password == "gbgmerch2025")
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