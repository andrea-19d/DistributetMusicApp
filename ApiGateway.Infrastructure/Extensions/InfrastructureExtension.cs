using ApiGateway.Application.Abstract;
using ApiGateway.Domain.Entities;
using ApiGateway.Infrastructure.Repository;
using ApiGateway.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ApiGateway.Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        return services;
    }
    
    // private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    // {
    //     var connectionString = configuration.GetConnectionString("DefaultConnection");
    //     if (string.IsNullOrWhiteSpace(connectionString))
    //         throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    //
    //     services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
    //
    //     services.AddSingleton<GatewayDbContext>(sp =>
    //     {
    //         var client = sp.GetRequiredService<IMongoClient>();
    //         return new GatewayDbContext(connectionString);
    //     });
    //
    //     return services;
    // }
    
    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoDbSettings>(provider =>
            provider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        
        services.AddScoped<IMongoRepository<ApiRoute>, MongoRepository<ApiRoute>>();
        return services;
    }
}