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
        [HttpPost]
        public IActionResult Clear()
        {
            _cartService.ClearCart();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult PlaceOrder(string FullName, string Email, string Street, string City, string PostalCode, string Country)
        {
            // I framtiden kan du spara ordern i databasen här
            _cartService.ClearCart();

            TempData["OrderMessage"] = $"Tack för din beställning, {FullName}! Vi skickar till {Street}, {PostalCode} {City}, {Country}.";

            return RedirectToAction("Confirmation");
        }
        public IActionResult Confirmation()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            var items = _cartService.GetCartItems();
            // Här kan du skicka med en ViewModel med kundinfo + cartitems
            return View(items);
        }
       [HttpGet]
        public IActionResult GetCartCount()
        {
            var items = _cartService.GetCartItems();
            var totalCount = items.Sum(item => item.quantity);
            return Json(totalCount);
        }
        [HttpPost]
        public IActionResult UpdateQuantity(Guid productId, int change)
        {
            _cartService.UpdateQuantity(productId, change);
            return RedirectToAction("Index");
        }



    }
}
