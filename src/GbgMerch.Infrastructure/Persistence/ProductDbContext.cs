using Microsoft.EntityFrameworkCore;
using GbgMerch.Domain.Entities;

namespace GbgMerch.Infrastructure.Persistence
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
// Compare this snippet from src/GbgMerch.Domain/Entities/Product.cs:
// using System.ComponentModel.DataAnnotations;