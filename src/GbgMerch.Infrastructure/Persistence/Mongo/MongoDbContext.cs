// src/GbgMerch.Infrastructure/Persistence/MongoDbContext.cs
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GbgMerch.Domain.Entities;
using GbgMerch.Infrastructure.Persistence.Mongo; // Där din MongoDbSettings ligger

namespace GbgMerch.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoDatabase Database => _database;
}
