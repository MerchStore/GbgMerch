using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GbgMerch.WebUI.ViewModels;

public class CreateProductViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 999999)]
    public decimal PriceAmount { get; set; }

    [Required]
    public string PriceCurrency { get; set; } = "SEK";

    [Range(0, 999999)]
    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    public string Tags { get; set; } = string.Empty; // komma-separerad string (t.ex. "cool,limited,summer")
}
