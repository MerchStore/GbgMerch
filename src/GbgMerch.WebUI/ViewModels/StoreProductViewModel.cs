using GbgMerch.Domain.Entities;

namespace GbgMerch.WebUI.Models;

public class StoreProductViewModel
{
    public Product Product { get; set; } = null!;
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
