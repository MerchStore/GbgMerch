using GbgMerch.Domain.Entities;
using GbgMerch.Domain.ValueObjects;
using GbgMerch.Infrastructure.Persistence.Mongo;
using MongoDB.Driver;

namespace GbgMerch.Infrastructure.Persistence.Seeding;

public class MongoDbSeeder
{
    private readonly MongoDbContext _context;

    public MongoDbSeeder(MongoDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var productsCollection = _context.Database.GetCollection<Product>("Products");

        // Kontrollera om några produkter redan finns
        var existingCount = await productsCollection.CountDocumentsAsync(_ => true);
        if (existingCount > 0)
            return;

        var products = new List<Product>
        {
            new Product(
                name: "Gbg Mug",
                description: "En snygg mugg med Göteborgsmotiv.",
                imageUrl: new Uri("https://gbgmerch.blob.core.windows.net/gbgmerch/Gbg%20mug.png"),
                price: Money.FromSEK(129),
                stockQuantity: 100,
                category: "Mugs",
                tags: new List<string> { "souvenir", "ceramic", "coffee" }),

            new Product(
                name: "Gbg T-Shirt",
                description: "T-shirt med tryck från Göteborg.",
                imageUrl: new Uri("https://gbgmerch.blob.core.windows.net/gbgmerch/T-shirt.png"),
                price: Money.FromSEK(249),
                stockQuantity: 50,
                category: "Clothing",
                tags: new List<string> { "tshirt", "fashion", "limited" }),

            new Product(
                name: "Gbg Hat",
                description: "Keps med Göteborgslogga.",
                imageUrl: new Uri("https://gbgmerch.blob.core.windows.net/gbgmerch/hatGbg.png"),
                price: Money.FromSEK(199),
                stockQuantity: 30,
                category: "Accessories",
                tags: new List<string> { "hat", "summer", "popular" })
        };

        await productsCollection.InsertManyAsync(products);
    }

}
