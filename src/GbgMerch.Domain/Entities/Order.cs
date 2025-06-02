using GbgMerch.Domain.Entities;
namespace GbgMerch.Domain.Entities
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Status { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalAmount { get; set; }
        public string ShippingStreet { get; set; } = string.Empty;
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingPostalCode { get; set; } = string.Empty;
        public string ShippingCountry { get; set; } = string.Empty;


        public Order(string customerName, string customerEmail, string status,
             List<OrderItem> items, decimal totalAmount,
             string shippingStreet, string shippingCity, string shippingPostalCode, string shippingCountry)
        {
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            Status = status;
            Items = items;
            TotalAmount = totalAmount;
            ShippingStreet = shippingStreet;
            ShippingCity = shippingCity;
            ShippingPostalCode = shippingPostalCode;
            ShippingCountry = shippingCountry;
        }

    }
}
