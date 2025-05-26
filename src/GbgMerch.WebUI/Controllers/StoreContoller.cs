using GbgMerch.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GbgMerch.WebUI.Models;
using GbgMerch.Application.Services.Interfaces;


namespace GbgMerch.WebUI.Controllers;

public class StoreController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewService _reviewService;

    public StoreController(IProductRepository productRepository, IReviewService reviewService)
    {
        _productRepository = productRepository;
        _reviewService = reviewService;
    }


    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllAsync();

        var viewModelList = new List<StoreProductViewModel>();

        foreach (var product in products)
        {
            var averageRating = await _reviewService.GetAverageRatingForProductAsync(product.Id);
            var reviewCount = await _reviewService.GetReviewCountForProductAsync(product.Id);

            viewModelList.Add(new StoreProductViewModel
            {
                Product = product,
                AverageRating = averageRating,
                ReviewCount = reviewCount
            });
        }

        return View(viewModelList);
    }


    public async Task<IActionResult> Details(string id)
    {
        if (!Guid.TryParse(id, out Guid guidId))
            return BadRequest("Ogiltigt ID");

        var product = await _productRepository.GetByIdAsync(guidId);
        if (product is null)
            return NotFound();

        var reviews = (await _reviewService.GetReviewsByProductIdAsync(product.Id)).ToList();
        var avg = await _reviewService.GetAverageRatingForProductAsync(product.Id);
        var count = await _reviewService.GetReviewCountForProductAsync(product.Id);

        var viewModel = new ProductReviewViewModel
        {
            Product = product,
            Reviews = reviews,
            AverageRating = avg,
            ReviewCount = count
        };

        return View(viewModel);
    }



}
