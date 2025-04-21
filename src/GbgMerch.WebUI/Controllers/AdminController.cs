using Microsoft.AspNetCore.Mvc;
using GbgMerch.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GbgMerch.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;

        public AdminController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "You must be an admin to access the dashboard.";
                return RedirectToAction("Login", "Account");
            }

            return View("Dashboard");
        }

        public async Task<IActionResult> Products()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            var products = await _productRepository.GetAllAsync();
            return View(products); // -> Views/Admin/Products.cshtml
        }

        public IActionResult Orders()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            return View(); // Placeholder
        }

        public IActionResult Settings()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            return View(); // Placeholder
        }
    }
}
