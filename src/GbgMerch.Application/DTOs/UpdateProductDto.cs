namespace GbgMerch.Application.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string Currency { get; set; } = "SEK";
        public string? ImageUrl { get; set; }
        public int StockQuantity { get; set; }
    }
}