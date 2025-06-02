namespace GbgMerch.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();         // Unikt ID för varje OrderItem
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Räknas automatiskt: Pris * Antal
    public decimal Subtotal => Price * Quantity;
}
