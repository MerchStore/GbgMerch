using GbgMerch.Domain.Entities;

namespace GbgMerch.Application.Cart
{
    public class CartService : ICartService
    {
        private readonly List<(Product product, int quantity)> _item = new();
        private readonly Dictionary<Guid, (Product Product, int Quantity)> _items = new();

        public void AddToCart(Product product, int quantity)
        {
            if (_items.ContainsKey(product.Id))
            {
                var current = _items[product.Id];
                _items[product.Id] = (current.Product, current.Quantity + quantity);
            }
            else
            {
                _items[product.Id] = (product, quantity);
            }
        }

        public void UpdateQuantity(Guid productId, int change)
        {
            if (_items.ContainsKey(productId))
            {
                var current = _items[productId];
                var newQuantity = current.Quantity + change;

                if (newQuantity <= 0)
                    _items.Remove(productId);
                else
                    _items[productId] = (current.Product, newQuantity);
            }
        }



        public List<(Product product, int quantity)> GetCartItems()
        {
            return _items.Values.Select(x => (x.Product, x.Quantity)).ToList();
        }


        public void ClearCart()
        {
            _items.Clear();
        }
    }
}
