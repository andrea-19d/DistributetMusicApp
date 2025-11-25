using CatalogService.Application.Abstract;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Repository;

public class MongoReadRepository<T> : IMongoReadRepository<T> where T : MongoDocument
{
    private readonly IMongoCollection<T> _collection;

    public MongoReadRepository(IMongoDbSettings settings, IMongoClient client)
    {
        var db = client.GetDatabase(settings.DatabaseName);
        var tableName = typeof(T).Name.ToLower();

        _collection = db
            .WithReadPreference(ReadPreference.SecondaryPreferred)
            .GetCollection<T>(tableName);
    }

    public async Task<List<T>> GetAllAsync()
    {
        var cursor = await _collection.FindAsync(new BsonDocument());
        return await cursor.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        try
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}