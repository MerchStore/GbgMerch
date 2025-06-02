using GbgMerch.Application.Cart;
using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;
using GbgMerch.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GbgMerch.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly OrderRepository _orderRepository;

        public CartController(ICartService cartService, IProductRepository productRepository, OrderRepository orderRepository)
        {
            _cartService = cartService;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
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
        public async Task<IActionResult> PlaceOrder(string FullName, string Email, string Street, string City, string PostalCode, string Country)
        {
            var cart = _cartService.GetCartItems();

            var orderItems = cart.Select(item => new OrderItem
            {
                ProductName = item.product.Name,
                Quantity = item.quantity,
                UnitPrice = item.product.Price.Amount
            }).ToList();

            var total = orderItems.Sum(i => i.UnitPrice * i.Quantity);

            var order = new Order(
                FullName,
                Email,
                "Received",
                orderItems,
                total,
                Street,
                City,
                PostalCode,
                Country
            );

            // 🟡 Lägg till dessa loggar:
            Console.WriteLine("📦 Försöker spara order till MongoDB...");
            await _orderRepository.SaveAsync(order);
            Console.WriteLine("✅ Order skickad till MongoDB!");

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
