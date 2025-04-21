using GbgMerch.Infrastructure.Persistence;
using GbgMerch.Infrastructure.Persistence.Seeding;
using Microsoft.Extensions.Options;
using GbgMerch.Infrastructure.Persistence.Mongo; // för MongoDbSettings
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GbgMerch.Domain.Interfaces;
namespace GbgMerch.Infrastructure;

public static class DependencyInjection
{
   public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton<MongoDbContext>();

    
    services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));

    // Registrera Mongo-repository
    services.AddScoped<IProductRepository, MongoProductRepository>();

    // 🔧 Registrera seeder för testdata
    services.AddScoped<MongoDbSeeder>();

    return services;
}

}
