/*
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
            Name = "Sticker Pack",
            Description = "5 programming language stickers.",
            Price = 79.99m,
            StockQuantity = 200,
            ImageUrl = "/images/stickers.png"
        }
    };

    public Task AddAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    // Returnerar alla produkter i listan (används i StoreController)
    public Task<List<Product>> GetAllAsync() => Task.FromResult(_products);

    // Hämta en enskild produkt baserat på ID
    public Task<Product?> GetByIdAsync(Guid id)
        => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<Product?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    Task IProductRepository.GetByIdAsync(Guid id)
    {
        return GetByIdAsync(id);
    }
}
*/

