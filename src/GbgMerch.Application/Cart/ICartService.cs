using GbgMerch.Domain.Entities;

namespace GbgMerch.Application.Cart
{
    public interface ICartService
    {
        void AddToCart(Product product, int quantity);
        List<(Product product, int quantity)> GetCartItems();
        void ClearCart();
        void UpdateQuantity(Guid productId, int change);
    }
}
