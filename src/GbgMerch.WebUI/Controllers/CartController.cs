using GbgMerch.Application.Cart;
using GbgMerch.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GbgMerch.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;

        public CartController(ICartService cartService, IProductRepository productRepository)
        {
            _cartService = cartService;
            _productRepository = productRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                return NotFound();

            _cartService.AddToCart(product, quantity);
            return RedirectToAction("Details", "Store", new { id = productId });

        }
        public IActionResult Index()
        {
            var items = _cartService.GetCartItems();
            return View(items);
        }

    }
}
