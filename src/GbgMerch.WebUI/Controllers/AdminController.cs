using Microsoft.AspNetCore.Mvc;

namespace GbgMerch.WebUI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // Kolla om admin är inloggad
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToAction("Login", "Account");
            }

            // Visa admin-dashboard
            return View();
        }
    }
}
