using CatalogService.Application.Abstract;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Settings;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Repository;

public class MongoWriteRepository<T> : IMongoWriteRepository<T> where T : MongoDocument
{
    private readonly IMongoCollection<T> _collection;

    public MongoWriteRepository(IMongoDbSettings settings, IMongoClient client)
    {
        var db = client.GetDatabase(settings.DatabaseName);
        var tableName = typeof(T).Name.ToLower();

        _collection = db
            .WithReadPreference(ReadPreference.Primary)
            .GetCollection<T>(tableName);
    }

    public async Task<T> InsertAsync(T entity)
    {
        entity.LastUpdated = DateTime.UtcNow;
        await _collection.InsertOneAsync(entity);
       
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        entity.LastUpdated = DateTime.UtcNow;
        var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}