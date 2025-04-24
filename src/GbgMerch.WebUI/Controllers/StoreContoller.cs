using GbgMerch.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GbgMerch.WebUI.Controllers;

public class StoreController : Controller
{
    private readonly IProductRepository _productRepository;

    public StoreController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllAsync();
        return View(products);
    }
    public async Task<IActionResult> Details(string id)
    {
    if (!Guid.TryParse(id, out Guid guidId))
        return BadRequest("Ogiltigt ID");

    var product = await _productRepository.GetByIdAsync(guidId);

    return View(product);
    }


}
