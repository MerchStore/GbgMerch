using System;
using System.ComponentModel.DataAnnotations;

namespace GbgMerch.WebUI.Models;

public class EditProductViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    [Range(0.01, 999999)]
    public decimal PriceAmount { get; set; }

    [Required]
    public string PriceCurrency { get; set; } = "SEK";

    [Range(0, 999999)]
    public int StockQuantity { get; set; }

    [Required]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    public string Tags { get; set; } = string.Empty;
}
