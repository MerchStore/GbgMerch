using GbgMerch.Domain.Entities;
using GbgMerch.Domain.ValueObjects;

namespace GbgMerch.Domain.UnitTests.Entities;

public class ProductTests
{
    private Product CreateValidProduct()
    {
        return new Product(
            "Test Product",
            "Test Description",
            new Uri("https://example.com/image.jpg"),
            new Money(19.99m, "USD"),
            10,
            "Accessories",
            new List<string> { "gift", "popular" }
        );
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesProduct()
    {
        // Arrange
        string name = "Test Product";
        string description = "Test Description";
        var imageUrl = new Uri("https://example.com/image.jpg");
        var price = new Money(19.99m, "USD");
        int stockQuantity = 10;
        string category = "Accessories";
        var tags = new List<string> { "tag1", "tag2" };

        // Act
        var product = new Product(name, description, imageUrl, price, stockQuantity, category, tags);

        // Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(imageUrl, product.ImageUrl);
        Assert.Equal(price, product.Price);
        Assert.Equal(stockQuantity, product.StockQuantity);
        Assert.Equal(category, product.Category);
        Assert.Equal(tags, product.Tags);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Theory]
    [InlineData("", "Test Description", "name")]
    [InlineData(null, "Test Description", "name")]
    [InlineData("Test Product", "", "description")]
    [InlineData("Test Product", null, "description")]
    public void Constructor_WithInvalidNameOrDescription_ThrowsArgumentException(string? name, string? description, string paramName)
    {
        var imageUrl = new Uri("https://example.com/image.jpg");
        var price = new Money(19.99m, "USD");
        int stockQuantity = 10;
        string category = "Accessories";
        var tags = new List<string> { "tag" };

        var exception = Assert.Throws<ArgumentException>(() =>
            new Product(name!, description!, imageUrl, price, stockQuantity, category, tags));

        Assert.Equal(paramName, exception.ParamName);
    }

    [Theory]
    [InlineData("A very long product name that exceeds the maximum allowed length of 100 characters which is meant to test validation logic", "Test Description", "name")]
    [InlineData("Test Product", "A very long product description that exceeds the maximum allowed length. It goes on and on with unnecessary details and filler content just to make sure we hit the 500 character limit that we've set for our validation logic. It keeps going with more and more text that doesn't really add any value but just takes up space to ensure we exceed the limit. We're adding even more text here to make absolutely certain that this description is too long for our product entity. This should definitely trigger the validation logic that checks for description length and throw an appropriate exception with the correct parameter name to help developers identify and fix the issue quickly.", "description")]
    public void Constructor_WithTooLongNameOrDescription_ThrowsArgumentException(string name, string description, string paramName)
    {
        var imageUrl = new Uri("https://example.com/image.jpg");
        var price = new Money(19.99m, "USD");
        int stockQuantity = 10;
        string category = "Accessories";
        var tags = new List<string> { "tag" };

        var exception = Assert.Throws<ArgumentException>(() =>
            new Product(name, description, imageUrl, price, stockQuantity, category, tags));

        Assert.Equal(paramName, exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullPrice_ThrowsArgumentNullException()
    {
        string name = "Test Product";
        string description = "Test Description";
        var imageUrl = new Uri("https://example.com/image.jpg");
        Money price = null!;
        int stockQuantity = 10;
        string category = "Accessories";
        var tags = new List<string> { "tag" };

        Assert.Throws<ArgumentNullException>(() =>
            new Product(name, description, imageUrl, price, stockQuantity, category, tags));
    }

    [Fact]
    public void Constructor_WithNegativeStockQuantity_ThrowsArgumentException()
    {
        var imageUrl = new Uri("https://example.com/image.jpg");
        var price = new Money(19.99m, "USD");

        Assert.Throws<ArgumentException>(() =>
            new Product("Test", "Test", imageUrl, price, -5, "Accessories", new List<string> { "tag" }));
    }

    [Fact]
    public void Constructor_WithInvalidImageUrl_ThrowsArgumentException()
    {
        var imageUrl = new Uri("ftp://invalid.com/file.exe");
        var price = new Money(19.99m, "USD");

        var exception = Assert.Throws<ArgumentException>(() =>
            new Product("Test", "Test", imageUrl, price, 10, "Accessories", new List<string> { "tag" }));

        Assert.Contains("HTTP or HTTPS", exception.Message);
    }

    [Fact]
    public void UpdateDetails_WithValidParameters_UpdatesProduct()
    {
        var product = CreateValidProduct();
        var newImage = new Uri("https://example.com/new.jpg");

        product.UpdateDetails("New Name", "New Description", newImage);

        Assert.Equal("New Name", product.Name);
        Assert.Equal("New Description", product.Description);
        Assert.Equal(newImage, product.ImageUrl);
    }

    [Theory]
    [InlineData("", "Updated Description", "name")]
    [InlineData(null, "Updated Description", "name")]
    [InlineData("Updated Product", "", "description")]
    [InlineData("Updated Product", null, "description")]
    public void UpdateDetails_WithInvalidParameters_ThrowsArgumentException(string? newName, string? newDescription, string paramName)
    {
        var product = CreateValidProduct();
        var imageUrl = new Uri("https://example.com/image.jpg");

        var ex = Assert.Throws<ArgumentException>(() =>
            product.UpdateDetails(newName!, newDescription!, imageUrl));

        Assert.Equal(paramName, ex.ParamName);
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_UpdatesPrice()
    {
        var product = CreateValidProduct();
        var newPrice = new Money(49.99m, "USD");

        product.UpdatePrice(newPrice);

        Assert.Equal(newPrice, product.Price);
    }

    [Fact]
    public void UpdatePrice_WithNullPrice_ThrowsArgumentNullException()
    {
        var product = CreateValidProduct();
        Assert.Throws<ArgumentNullException>(() => product.UpdatePrice(null!));
    }

    [Fact]
    public void UpdateStock_WithValidQuantity_UpdatesStockQuantity()
    {
        var product = CreateValidProduct();
        product.UpdateStock(99);
        Assert.Equal(99, product.StockQuantity);
    }

    [Fact]
    public void UpdateStock_WithNegativeQuantity_ThrowsArgumentException()
    {
        var product = CreateValidProduct();
        Assert.Throws<ArgumentException>(() => product.UpdateStock(-5));
    }

    [Fact]
    public void DecrementStock_WithValidQuantity_DecreasesStock()
    {
        var product = CreateValidProduct();
        var result = product.DecrementStock(3);
        Assert.True(result);
        Assert.Equal(7, product.StockQuantity);
    }

    [Fact]
    public void DecrementStock_WithDefaultQuantity_DecreasesByOne()
    {
        var product = CreateValidProduct();
        product.DecrementStock();
        Assert.Equal(9, product.StockQuantity);
    }

    [Fact]
    public void DecrementStock_WithInsufficientStock_ReturnsFalse()
    {
        var product = CreateValidProduct();
        var result = product.DecrementStock(999);
        Assert.False(result);
        Assert.Equal(10, product.StockQuantity);
    }

    [Fact]
    public void DecrementStock_WithNegativeQuantity_ThrowsArgumentException()
    {
        var product = CreateValidProduct();
        Assert.Throws<ArgumentException>(() => product.DecrementStock(-1));
    }

    [Fact]
    public void IncrementStock_WithValidQuantity_IncreasesStock()
    {
        var product = CreateValidProduct();
        product.IncrementStock(5);
        Assert.Equal(15, product.StockQuantity);
    }

    [Fact]
    public void IncrementStock_WithZeroQuantity_ThrowsArgumentException()
    {
        var product = CreateValidProduct();
        Assert.Throws<ArgumentException>(() => product.IncrementStock(0));
    }

    [Fact]
    public void IncrementStock_WithNegativeQuantity_ThrowsArgumentException()
    {
        var product = CreateValidProduct();
        Assert.Throws<ArgumentException>(() => product.IncrementStock(-1));
    }

    [Fact]
    public void UpdateCategoryAndTags_WithValidValues_UpdatesSuccessfully()
    {
        var product = CreateValidProduct();

        var newCategory = "T-Shirts";
        var newTags = new List<string> { "sale", "summer" };

        product.UpdateCategoryAndTags(newCategory, newTags);

        Assert.Equal(newCategory, product.Category);
        Assert.Equal(newTags, product.Tags);
    }
}
