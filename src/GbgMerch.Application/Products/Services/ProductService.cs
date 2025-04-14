using GbgMerch.Application.DTOs;
using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;

namespace GbgMerch.Application.Products.Services;

// Tjänstelager för att hantera affärslogik kopplat till produkter
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    // Hämta alla produkter
    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            StockQuantity = p.StockQuantity
        }).ToList();
    }

    // Hämta en produkt med ID
    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            StockQuantity = product.StockQuantity
        };
    }

    // Lägg till ny produkt
    public async Task AddAsync(ProductDto dto)
    {
        var product = new Product
        {
            Id = 0, // Assuming the database will auto-generate the ID
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            StockQuantity = dto.StockQuantity
        };

        await _repository.AddAsync(product);
    }

    // Uppdatera produkt
    public async Task UpdateAsync(ProductDto dto)
    {
        var product = new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            StockQuantity = dto.StockQuantity
        };

        await _repository.UpdateAsync(product);
    }

    // Ta bort produkt
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}
