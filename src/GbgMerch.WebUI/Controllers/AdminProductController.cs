using GbgMerch.Application.Products.Services;
using GbgMerch.Domain.Entities;
using GbgMerch.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class AdminProductController : Controller
{

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Products");
    }

    [HttpPost]
    public IActionResult Create(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return RedirectToAction("Products");
    }
    [HttpGet]
    public IActionResult Products()
    {
        // Här laddar du listan av produkter
        return View(); // OBS: måste matcha rätt vy i /Views/Admin/Products.cshtml
    }

}