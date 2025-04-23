using Microsoft.AspNetCore.Mvc;
using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;
using GbgMerch.Domain.ValueObjects;
using GbgMerch.WebUI.Models;

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
            return View(products);
        }

        public async Task<IActionResult> ViewProduct(Guid id)
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return NotFound();

            return View("ViewProduct", product);
        }

        // ✅ GET: Edit
        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return NotFound();

            var viewModel = new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl?.ToString() ?? "",
                PriceAmount = product.Price.Amount,
                PriceCurrency = product.Price.Currency,
                StockQuantity = product.StockQuantity
            };

            return View("EditProduct", viewModel);
        }

        // ✅ POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductViewModel formModel)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View("EditProduct", formModel);

            var product = await _productRepository.GetByIdAsync(formModel.Id);
            if (product is null) return NotFound();

            try
            {
                product.UpdateDetails(
                    formModel.Name,
                    formModel.Description,
                    string.IsNullOrWhiteSpace(formModel.ImageUrl) ? null : new Uri(formModel.ImageUrl)
                );

                product.UpdatePrice(Money.Create(formModel.PriceAmount, formModel.PriceCurrency));
                product.UpdateStock(formModel.StockQuantity);

                await _productRepository.UpdateAsync(product);
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Failed to update product: {ex.Message}");
                return View("EditProduct", formModel);
            }
        }

        public IActionResult Orders()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Settings()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Unauthorized access.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}
