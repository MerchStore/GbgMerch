using GbgMerch.Domain.Entities;

namespace GbgMerch.Domain.Interfaces;

// Specifikt interface för produktrelaterade operationer
public interface IProductRepository : IRepository<Product, Guid>
{
    // Extra metodexempel kan läggas till här om nödvändigt i framtiden
    Task DeleteAsync(Product product);
}
