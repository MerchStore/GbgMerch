using GbgMerch.Domain.Entities;
using GbgMerch.Infrastructure.Persistence;
using MongoDB.Driver;

namespace GbgMerch.Infrastructure.Repositories;

public class OrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public OrderRepository(MongoDbContext context)
    {
        var database = context.Database;
        _orders = database.GetCollection<Order>("Order");
    }

    public async Task SaveAsync(Order order)
    {
        await _orders.InsertOneAsync(order);
    }
    public async Task<List<Order>> GetAllAsync()
    {
        return await _orders.Find(_ => true).ToListAsync();
    }
    public async Task<Order?> GetByIdAsync(string id)
        {
            return await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

    public async Task UpdateAsync(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
        }
}
