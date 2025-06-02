using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GbgMerch.WebUI.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = "SEK";

        [Required]
        [Range(0, 10000)]
        public int Stock { get; set; }

        [Url]
        [StringLength(2000)]
        public string? ImageUrl { get; set; }

        // 🆕 Nytt fält för kategori
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        // 🆕 Lista med taggar (komma-separerad input i View)
        public string Tags { get; set; } = string.Empty;
    }
}
