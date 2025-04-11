namespace GbgMerch.Domain.Entities;

public class Product
{
    public int Id { get; set; } // Unikt ID för produkten

    public string Name { get; set; } = string.Empty; // Produktens namn

    public string Description { get; set; } = string.Empty; // Kort beskrivning

    public decimal Price { get; set; } // Pris i SEK

    public int StockQuantity { get; set; } // Antal i lager

    public string ImageUrl { get; set; } = string.Empty; // Länk till produktbild
    
}
