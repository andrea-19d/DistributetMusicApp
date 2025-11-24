namespace CatalogService.Infrastructure.Settings;

public interface IMongoDbSettings
{
    string DatabaseName { get; }
    string ConnectionString { get; }
}