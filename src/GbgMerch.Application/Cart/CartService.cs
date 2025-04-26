using GbgMerch.Domain.Entities;

namespace GbgMerch.Application.Cart
{
    public class CartService : ICartService
    {
        private readonly List<(Product product, int quantity)> _item = new();

        public void AddToCart(Product product, int quantity)
        {
            for (int i = 0; i < _item.Count; i++)
            {
                if (_item[i].product.Id == product.Id)
                {
                    _item[i] = (_item[i].product, _item[i].quantity + quantity);
                    return;
                }
            }

            _item.Add((product, quantity));
        }

        public List<(Product product, int quantity)> GetCartItems()
        {
            return _item;
        }

        public void ClearCart()
        {
            _item.Clear();
        }
    }
}
