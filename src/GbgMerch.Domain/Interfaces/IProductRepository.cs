using GbgMerch.Domain.Entities;

namespace GbgMerch.Domain.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    Task GetByIdAsync(Guid id);
}
