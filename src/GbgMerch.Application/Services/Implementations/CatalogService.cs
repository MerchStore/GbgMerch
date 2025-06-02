using GbgMerch.Application.DTOs;
using GbgMerch.Application.Services.Interfaces;
using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;
using GbgMerch.Domain.ValueObjects;

namespace GbgMerch.Application.Services.Implementations;

/// <summary>
/// Implementation of the catalog service.
/// Acts as a facade over the repository layer.
/// </summary>
public class CatalogService : ICatalogService
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    public CatalogService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    /// <inheritdoc/>
    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }
    public async Task<Guid> CreateProductAsync(CreateProductDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Product name cannot be empty.", nameof(dto.Name));
        }

        if (dto.Price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero.", nameof(dto.Price));
        }

        if (dto.StockQuantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(dto.StockQuantity));
        }
    {
        var product = new Product(
        name: dto.Name,
        description: dto.Description,
        imageUrl: string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : new Uri(dto.ImageUrl),
        price: new Money(dto.Price, dto.Currency),
        stockQuantity: dto.StockQuantity,
        category: "Default", // eller något du har tillgängligt i kontexten
        tags: new List<string> ()
        );

        await _productRepository.AddAsync(product);

        return product.Id;
    }
    }
    public async Task<bool> UpdateProductAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (dto is null)
            return false;

        if (product is null)
            return false;


        Uri? imageUri = null;
        if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
        {
            if (Uri.TryCreate(dto.ImageUrl, UriKind.Absolute, out var parsedUri))
                imageUri = parsedUri;
            else
                throw new ArgumentException("Ogiltig bildlänk", nameof(dto.ImageUrl));
        }

        product.UpdateDetails(dto.Name, dto.Description, imageUri);
        product.UpdatePrice(new Money(dto.Price, dto.Currency));
        product.UpdateStock(dto.StockQuantity);

        await _productRepository.UpdateAsync(product);

        return true;

    }
    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return false;

        await _productRepository.DeleteAsync(product);
        return true;
    }



}