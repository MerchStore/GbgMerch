using Microsoft.AspNetCore.Mvc;
using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;

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
            return View(products); // Views/Admin/Products.cshtml
        }

        // ✅ Visa en enskild produkt
        public async Task<IActionResult> ViewProduct(Guid id)
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return NotFound();

            return View("ViewProduct", product); // Views/Admin/ViewProduct.cshtml
        }

        // ✅ GET: Visa produkt för redigering
        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid id)
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return NotFound();

            return View("EditProduct", product); // Views/Admin/EditProduct.cshtml
        }

        // ✅ POST: Uppdatera produkten
        [HttpPost]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> EditProduct(Product updated)
{
    if (!IsAdmin()) return RedirectToAction("Login", "Account");

    if (!ModelState.IsValid)
        return View("EditProduct", updated);

    await _productRepository.UpdateAsync(updated);
    return RedirectToAction("Products");
}

        public IActionResult Orders()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            return View(); // Views/Admin/Orders.cshtml
        }

        public IActionResult Settings()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            return View(); // Views/Admin/Settings.cshtml
        }
    }
}
