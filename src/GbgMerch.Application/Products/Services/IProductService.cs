using GbgMerch.Application.DTOs; // Ensure this namespace contains the ProductViewModel class


namespace GbgMerch.Application.Products.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();                  // Hämta alla produkter
    Task<ProductDto?> GetByIdAsync(Guid id);               // Hämta en produkt med ID
    Task AddAsync(ProductDto dto);                         // Lägg till ny produkt
    Task UpdateAsync(ProductDto dto);                      // Uppdatera produkt
    Task DeleteAsync(Guid id);                             // Ta bort produkt
}
