using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;

namespace GbgMerch.Infrastructure.Persistence.Repositories;

// Denna klass implementerar IProductRepository och fungerar som en fejkad datakälla (in-memory)
public class InMemoryProductRepository : IProductRepository
{
    // Lista med fejkade produkter, som vi använder för att visa i Store-vyn
    private static readonly List<Product> _products = new()
    {
        new Product 
        {
            Id = Guid.NewGuid(), // unikt ID för varje produkt
            Name = "Conference T-Shirt",
            Description = "A comfortable cotton t-shirt.",
            Price = 249.99m,
            StockQuantity = 50,
            ImageUrl = "/images/tshirt.png"
        },
        new Product 
        {
            Id = Guid.NewGuid(),
            Name = "Developer Mug",
            Description = "A mug with a funny dev joke.",
            Price = 149.50m,
            StockQuantity = 100,
            ImageUrl = "/images/mug.png"
        },
        new Product 
        {
            Id = Guid.NewGuid(),
            Name = "Hat",
            Description = "Clo special design hat.",
            Price = 299.99m,
            StockQuantity = 200,
            ImageUrl = "/images/hat.png"
        }
    };

    // Returnerar alla produkter i listan (används i StoreController)
    public Task<List<Product>> GetAllAsync() => Task.FromResult(_products);

    // Hämta en enskild produkt baserat på ID
    public Task<Product?> GetByIdAsync(Guid id)
        => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
}
