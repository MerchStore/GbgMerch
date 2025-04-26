using System;
using System.ComponentModel.DataAnnotations;

namespace GbgMerch.WebUI.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; } // används vid redigering

        [Required]
        [StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "Description must be under 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "Currency must be 3 characters.")]
        public string Currency { get; set; } = "SEK";

        [Required]
        [Range(0, 10000, ErrorMessage = "Stock must be a non-negative number.")]
        public int Stock { get; set; }

        [Url]
        [StringLength(2000, ErrorMessage = "Image URL is too long.")]
        public string? ImageUrl { get; set; }
    }
}
