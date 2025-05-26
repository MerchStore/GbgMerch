using Microsoft.AspNetCore.Mvc;
using GbgMerch.Application.Services.Interfaces;
using GbgMerch.WebUI.Models.Catalog;
using GbgMerch.Application.Services.Interfaces;


namespace GbgMerch.WebUI.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly IReviewService _reviewService;

    public CatalogController(ICatalogService catalogService, IReviewService reviewService)
    {
        _catalogService = catalogService;
        _reviewService = reviewService;
    }


    // GET: Catalog
    public async Task<IActionResult> Index()
{
    try
    {
        // 1. Hämta produkter från catalogService
        var products = await _catalogService.GetAllProductsAsync();

        // 🟡 Här börjar vi skapa ViewModel-listan
        var productViewModels = new List<ProductCardViewModel>();

        // 2. För varje produkt – hämta betyg & antal reviews via ReviewService
        foreach (var p in products)
        {
            var rating = await _reviewService.GetAverageRatingForProductAsync(p.Id);
            var count = await _reviewService.GetReviewCountForProductAsync(p.Id);

            productViewModels.Add(new ProductCardViewModel
            {
                Id = p.Id,
                Name = p.Name,
                TruncatedDescription = p.Description.Length > 100
                    ? p.Description.Substring(0, 97) + "..."
                    : p.Description,
                FormattedPrice = p.Price.ToString(),
                PriceAmount = p.Price.Amount,
                ImageUrl = p.ImageUrl?.ToString(),
                StockQuantity = p.StockQuantity,
                AverageRating = rating,        // ✅ Här läggs in
                ReviewCount = count            // ✅ Här läggs in
            });
        }

        // 3. Skapa huvud-vymodellen med alla produktkort
        var viewModel = new ProductCatalogViewModel
        {
            FeaturedProducts = productViewModels
        };

        return View(viewModel);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in ProductCatalog: {ex.Message}");
        ViewBag.ErrorMessage = "An error occurred while loading products. Please try again later.";
        return View("Error");
    }
}


    // GET: Store/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            // Get the specific product from the service
            var product = await _catalogService.GetProductByIdAsync(id);

            // Return 404 if product not found
            if (product is null)
            {
                return NotFound();
            }

            // Map domain entity to view model
            var viewModel = new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                FormattedPrice = product.Price.ToString(),
                PriceAmount = product.Price.Amount,
                ImageUrl = product.ImageUrl?.ToString(),
                StockQuantity = product.StockQuantity
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in ProductDetails: {ex.Message}");

            // Show an error message to the user
            ViewBag.ErrorMessage = "An error occurred while loading the product. Please try again later.";
            return View("Error");
        }
    }
}