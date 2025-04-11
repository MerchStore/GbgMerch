using GbgMerch.Application.Common.Interfaces;
using GbgMerch.Application.DTOs;
using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;

namespace GbgMerch.Application.Products.Services;

// Implementerar produkt-tjänster
public class ProductService : IProductService
{
    private readonly IProductRepository _repository; // Datatillgång

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _repository.GetAllAsync();

        // Mappa till DTO-format
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        }).ToList();
    }

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
            ImageUrl = product.ImageUrl
        };
    }
}
