using CatalogService.Application.Abstract;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Repository;
using CatalogService.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IMongoDbSettings>();

            var mongoSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
            return new MongoClient(mongoSettings);
        });
        
        return services;
    }
    
    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoDbSettings>(provider =>
            provider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        
        services.AddScoped<IMongoReadRepository<Album>, MongoReadRepository<Album>>();
        services.AddScoped<IMongoWriteRepository<Album>, MongoWriteRepository<Album>>();

        services.AddScoped<IMongoReadRepository<Track>, MongoReadRepository<Track>>();
        services.AddScoped<IMongoWriteRepository<Track>, MongoWriteRepository<Track>>();
        
        services.AddScoped<IMongoReadRepository<Playlist>, MongoReadRepository<Playlist>>();
        services.AddScoped<IMongoWriteRepository<Playlist>, MongoWriteRepository<Playlist>>();
    }
}