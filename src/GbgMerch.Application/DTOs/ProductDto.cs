namespace GbgMerch.Application.DTOs;

// DTO = Data Transfer Object, används för att skicka data mellan lager
public class ProductDto
{
    public int Id { get; set; }             // Produktens unika ID
    public string Name { get; set; } = null!; // Produktnamn
    public string Description { get; set; } = null!; // Kort beskrivning
    public decimal Price { get; set; }       // Pris
    public string ImageUrl { get; set; } = null!; // Länk till produktbild
}
