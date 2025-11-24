namespace CatalogService.Infrastructure.Settings;

public class MongoDbSettings : IMongoDbSettings
{
    public required string DatabaseName { get; set; }
    public required string ConnectionString { get; set; }
}