using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GbgMerch.Infrastructure.Persistence;
using GbgMerch.Infrastructure.Persistence.Seeding;
using GbgMerch.Infrastructure.Persistence.Mongo;
using GbgMerch.Domain.Interfaces;
using GbgMerch.Infrastructure.ExternalServices.Reviews;
using GbgMerch.Infrastructure.ExternalServices.Reviews.Configurations;
using GbgMerch.Application.Services.Interfaces;
using GbgMerch.Application.Services.Implementations;

namespace GbgMerch.Infrastructure;

public static class DependencyInjection
{
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Dela upp i moduler
        services.AddPersistenceServices(configuration);
        services.AddReviewServices(configuration);

        return services;
    }

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // MongoDB
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IProductRepository, MongoProductRepository>();
        services.AddScoped<MongoDbSeeder>();

        return services;
    }

    public static IServiceCollection AddReviewServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Externa Review API-inställningar
        services.Configure<ReviewApiOptions>(configuration.GetSection(ReviewApiOptions.SectionName));

        // HttpClient för API-anrop
        services.AddHttpClient<ReviewApiClient>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5)); // Valfritt

        // Mock eller riktig service – välj beroende på projekt
        services.AddSingleton<MockReviewService>();

        // Repository som anropar externa API:t
        services.AddScoped<IReviewRepository, ExternalReviewRepository>();
        services.AddScoped<IReviewService, ReviewService>();


        return services;
    }

    // Seeder-anrop kan göras från Program.cs
    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
        await seeder.SeedAsync();
    }
    
}
