using GbgMerch.Application.DTOs;

namespace GbgMerch.Application.Products.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetByIdAsync(int id); // OBS: int, inte Guid
    }
}
