using GbgMerch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GbgMerch.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
    }
}
