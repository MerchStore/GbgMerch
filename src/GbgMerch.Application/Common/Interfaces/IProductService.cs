using GbgMerch.Application.DTOs;

namespace GbgMerch.Application.Common.Interfaces;

// Interface för affärslogik relaterad till produkter
public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync(); // Hämtar alla produkter
    Task<ProductDto?> GetByIdAsync(Guid id);      // Hämtar en produkt med ID
}
