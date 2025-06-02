using GbgMerch.Application.Cart;
using Microsoft.AspNetCore.Mvc;

public class CartCountViewComponent : ViewComponent
{
    private readonly ICartService _cartService;

    public CartCountViewComponent(ICartService cartService)
    {
        _cartService = cartService;
    }

    public IViewComponentResult Invoke()
    {
        var count = _cartService.GetCartItems().Sum(item => item.quantity);
        return View(count);
    }
}
