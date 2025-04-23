namespace GbgMerch.WebUI.ViewModels;

public class ProductViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public string Currency { get; set; } = "SEK";
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}
