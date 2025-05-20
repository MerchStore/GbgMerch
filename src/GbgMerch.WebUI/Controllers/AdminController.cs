using Microsoft.AspNetCore.Mvc;
using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;
using GbgMerch.Domain.ValueObjects;
using GbgMerch.WebUI.ViewModels; // ✅ Se till att både EditProductViewModel och ProductViewModel finns här
using GbgMerch.WebUI.Models;
using GbgMerch.Infrastructure.Repositories;

namespace GbgMerch.WebUI.Controllers;

public class AdminController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly OrderRepository _orderRepository;

    public AdminController(IProductRepository productRepository, OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
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
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var products = await _productRepository.GetAllAsync();
        return View(products);
    }

    public async Task<IActionResult> ViewProduct(Guid id)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) return NotFound();

        return View("ViewProduct", product);
    }

    // 🛠️ CREATE
    [HttpGet]
    public IActionResult Create()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");
        return View("Create");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
            return View("CreateProduct", model);

        try
        {
                                var product = new Product(
                name: model.Name,
                description: model.Description,
                imageUrl: string.IsNullOrWhiteSpace(model.ImageUrl) ? null : new Uri(model.ImageUrl),
                price: Money.Create(model.PriceAmount, model.PriceCurrency),
                stockQuantity: model.StockQuantity,
                category: model.Category,
                tags: model.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
            );



            await _productRepository.AddAsync(product);
            return RedirectToAction("Products");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Could not create product: " + ex.Message);
            return View("CreateProduct", model);
        }
    }


    // 🛠️ EDIT
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
                StockQuantity = product.StockQuantity,
                Category = product.Category,
                Tags = string.Join(", ", product.Tags)
            };

        return View("EditProduct", viewModel);
    }

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

            // 🆕 Kategori och taggar
            product.UpdateCategoryAndTags(
                formModel.Category,
                formModel.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
            );


            await _productRepository.UpdateAsync(product);
            return RedirectToAction("Products");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Failed to update product: {ex.Message}");
            return View("EditProduct", formModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var orders = await _orderRepository.GetAllAsync();
        return View(orders);
    }


    public IActionResult Settings()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");
        return View();
    }

        [HttpGet]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) return NotFound();

        return View("DeleteProduct", product); // visar en bekräftelsesida
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelete(Guid id)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) return NotFound();

        await _productRepository.RemoveAsync(product);
        return RedirectToAction("Products");
    }
    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(string id, string newStatus)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();

        order.Status = newStatus;
        await _orderRepository.UpdateAsync(order); // Du behöver ha en UpdateAsync i repo

        return RedirectToAction("Orders");
    }
    [HttpGet]
    public async Task<IActionResult> ViewOrder(string id)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");

        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();

        return View("ViewOrder", order);
    }
}
