using GbgMerch.Application.DTOs;
using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;
using GbgMerch.Domain.ValueObjects;

namespace GbgMerch.Application.Products.Services;

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
            Price = p.Price.Amount,
            Currency = p.Price.Currency,
            ImageUrl = p.ImageUrl?.ToString() ?? "",
            StockQuantity = p.StockQuantity
        }).ToList();
    }

    // Hämta produkt med ID
    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.Amount,
            Currency = product.Price.Currency,
            ImageUrl = product.ImageUrl?.ToString() ?? "",
            StockQuantity = product.StockQuantity
        };
    }

    // Lägg till ny produkt
    public async Task AddAsync(ProductDto dto)
    {
        var price = new Money(dto.Price, dto.Currency);
        var imageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : new Uri(dto.ImageUrl);

        var product = new Product(
            name: dto.Name,
            description: dto.Description,
            imageUrl: imageUrl,
            price: price,
            stockQuantity: dto.StockQuantity
        );

        await _repository.AddAsync(product);
    }

    // Uppdatera produkt
    public async Task UpdateAsync(ProductDto dto)
    {
        var product = await _repository.GetByIdAsync(dto.Id);
        if (product is null) return;

        product.UpdateDetails(dto.Name, dto.Description,
            string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : new Uri(dto.ImageUrl));
        product.UpdatePrice(new Money(dto.Price, dto.Currency));
        product.UpdateStock(dto.StockQuantity);

        await _repository.UpdateAsync(product);
    }

    // Ta bort produkt
    public async Task DeleteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is not null)
        {
            await _repository.RemoveAsync(product);
        }
    }
}
