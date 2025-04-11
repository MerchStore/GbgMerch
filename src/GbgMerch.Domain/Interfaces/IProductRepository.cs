using GbgMerch.Domain.Entities;

namespace GbgMerch.Domain.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
}
