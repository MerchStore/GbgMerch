using GbgMerch.Domain.Entities;
using GbgMerch.Domain.Interfaces;
using GbgMerch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GbgMerch.Infrastructure.Persistence.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public EfProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
