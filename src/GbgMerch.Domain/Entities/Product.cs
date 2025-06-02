using GbgMerch.Domain.Common;
using GbgMerch.Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace GbgMerch.Domain.Entities;

[BsonIgnoreExtraElements]
public class Product : Entity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; } = Money.FromSEK(0);
    public int StockQuantity { get; private set; } = 0;
    public Uri? ImageUrl { get; private set; } = null;
    public string Category { get; private set; } = string.Empty;
    public List<string> Tags { get; private set; } = new();

    // ✅ Används av MongoDB via BsonClassMap
    public Product(Guid id, string name, string description, Uri? imageUrl, Money price, int stockQuantity, string category, List<string> tags)
        : base(id)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Price = price;
        StockQuantity = stockQuantity;
        Category = category;
        Tags = tags;
    }

    // ✅ Används när man skapar nya produkter via UI eller API
    public Product(string name, string description, Uri? imageUrl, Money price, int stockQuantity, string category, List<string> tags)
        : base(Guid.NewGuid())
    {
        // Validering
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name cannot be empty", nameof(name));
        if (name.Length > 100) throw new ArgumentException("Product name cannot exceed 100 characters", nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Product description cannot be empty", nameof(description));
        if (description.Length > 500) throw new ArgumentException("Product description cannot exceed 500 characters", nameof(description));
        if (imageUrl != null)
        {
            if (imageUrl.Scheme != "http" && imageUrl.Scheme != "https")
                throw new ArgumentException("Image URL must use HTTP or HTTPS", nameof(imageUrl));
            if (imageUrl.AbsoluteUri.Length > 2000)
                throw new ArgumentException("Image URL exceeds maximum length", nameof(imageUrl));
            string extension = Path.GetExtension(imageUrl.AbsoluteUri).ToLowerInvariant();
            if (!new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }.Contains(extension))
                throw new ArgumentException("Image URL must point to a valid image file", nameof(imageUrl));
        }
        if (price == null) throw new ArgumentNullException(nameof(price));
        if (stockQuantity < 0) throw new ArgumentException("Stock quantity cannot be negative", nameof(stockQuantity));
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category cannot be empty", nameof(category));
        if (tags == null) throw new ArgumentNullException(nameof(tags));
        if (tags.Count > 10) throw new ArgumentException("Too many tags. Maximum is 10.", nameof(tags));

        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Price = price;
        StockQuantity = stockQuantity;
        Category = category;
        Tags = tags;
    }

    // För EF Core (och Mongo om den inte använder BsonClassMap)
    private Product() { }

    public void UpdateDetails(string name, string description, Uri? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", nameof(name));
        if (name.Length > 100) throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty", nameof(description));
        if (description.Length > 500) throw new ArgumentException("Description cannot exceed 500 characters", nameof(description));

        if (imageUrl != null)
        {
            if (imageUrl.Scheme != "http" && imageUrl.Scheme != "https")
                throw new ArgumentException("Image URL must use HTTP or HTTPS", nameof(imageUrl));
            if (imageUrl.AbsoluteUri.Length > 2000)
                throw new ArgumentException("Image URL exceeds maximum length", nameof(imageUrl));
            string extension = Path.GetExtension(imageUrl.AbsoluteUri).ToLowerInvariant();
            if (!new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }.Contains(extension))
                throw new ArgumentException("Image URL must point to a valid image file", nameof(imageUrl));
        }

        Name = name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public void UpdateCategoryAndTags(string category, List<string> tags)
    {
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category cannot be empty", nameof(category));
        if (tags == null) throw new ArgumentNullException(nameof(tags));
        if (tags.Count > 10) throw new ArgumentException("Too many tags. Maximum is 10.", nameof(tags));

        Category = category;
        Tags = tags;
    }

    public void UpdatePrice(Money newPrice)
    {
        ArgumentNullException.ThrowIfNull(newPrice);
        Price = newPrice;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0) throw new ArgumentException("Stock quantity cannot be negative", nameof(quantity));
        StockQuantity = quantity;
    }

    public bool DecrementStock(int quantity = 1)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (StockQuantity < quantity) return false;
        StockQuantity -= quantity;
        return true;
    }

    public void IncrementStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        StockQuantity += quantity;
    }
}
