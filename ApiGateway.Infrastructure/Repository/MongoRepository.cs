using ApiGateway.Application.Abstract;
using ApiGateway.Domain.Entities;
using ApiGateway.Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiGateway.Infrastructure.Repository;

public class MongoRepository<T> : IMongoRepository<T> where T : MongoDocument
{
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoDbSettings settings)
    {
        _db = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);

        var tableName = typeof(T).Name switch
        {
            nameof(ApiRoute) => "api_routes", 
            _ => typeof(T).Name.ToLower()
        };

        _collection = _db.GetCollection<T>(tableName);
    }
    
    public async Task<List<T>> GetAllRecords()
    {
        var cursor = await _collection.FindAsync(new BsonDocument());
        var records = await cursor.ToListAsync();
        return records;
    }

    public T InsertRecords(T record)
    {
       record.LastUpdated = DateTime.UtcNow;
       _collection.InsertOne(record);
       Console.WriteLine($"Inserted {record.LastUpdated}");
       return record;
    }

    public T GetRecordById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void UpsertRecord(T record)
    {
        throw new NotImplementedException();
    }

    public void DeleteRecord(T record)
    {
        throw new NotImplementedException();
    }
}