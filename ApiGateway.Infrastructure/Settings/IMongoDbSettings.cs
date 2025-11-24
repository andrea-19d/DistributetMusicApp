namespace ApiGateway.Infrastructure.Settings;

public interface IMongoDbSettings
{
    string DatabaseName { get; }
    string ConnectionString { get; }
}