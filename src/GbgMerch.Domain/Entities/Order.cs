using GbgMerch.Domain.Entities;
namespace GbgMerch.Domain.Entities
{
    public class Order
    {
        public int Guid { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Status { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalAmount { get; set; }

        public Order(string CustomerName, string CustomerEmail, string Status, List<OrderItem> Items, decimal TotalAmount)
        {
            this.CustomerName = CustomerName;
            this.CustomerEmail = CustomerEmail;
            this.Status = Status;
            this.Items = Items;
            this.TotalAmount = TotalAmount;
        }
    }
}
