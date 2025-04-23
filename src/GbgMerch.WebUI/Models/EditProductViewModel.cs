namespace GbgMerch.WebUI.Models;

public class EditProductViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public decimal PriceAmount { get; set; }

    public string PriceCurrency { get; set; } = "SEK";

    public int StockQuantity { get; set; }
}
