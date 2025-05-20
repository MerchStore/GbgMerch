namespace GbgMerch.Application.DTOs;

// DTO = Data Transfer Object för produktdata mellan lager
public class ProductDto
{
    public Guid Id { get; set; }             // Produktens unika ID
    public string Name { get; set; } = "";   // Namn
    public string Description { get; set; } = ""; // Beskrivning
    public decimal Price { get; set; }       // Belopp (från Money)
    public string Currency { get; set; } = "SEK"; // Valuta (från Money)
    public string ImageUrl { get; set; } = "";    // URL som sträng (från Uri)
    public int StockQuantity { get; set; }   // Antal i lager
    public string Category { get; set; } = "";     // 🆕 Kategori
    public List<string> Tags { get; set; } = new(); // 🆕 Lista av taggar
}
