using GbgMerch.Application.DTOs;
using GbgMerch.Domain.Entities;

namespace GbgMerch.Application.Services.Interfaces;

/// <summary>
/// Service interface for Catalog-related operations.
/// Provides a simple abstraction over the repository layer.
/// </summary>
public interface ICatalogService
{
    /// <summary>
    /// Gets all available products
    /// </summary>
    /// <returns>A collection of all products</returns>
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Guid> CreateProductAsync(CreateProductDto dto);
    Task<bool> UpdateProductAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteProductAsync(Guid id);
    /// <summary>
    /// Gets a product by its unique identifier
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <returns>The product if found, null otherwise</returns>
    Task<Product?> GetProductByIdAsync(Guid id);
}