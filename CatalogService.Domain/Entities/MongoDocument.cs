using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace CatalogService.Domain.Entities;

public abstract class MongoDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string Id { get; set; } = default!;
    
    public DateTime LastUpdated { get; set; }
}