using System.Runtime.Serialization.Json;
using CacheManager.Core;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;

namespace ApiGateway.Api.Extensions;

public static class OcelotExtensions
{
    public static IServiceCollection AddOcelotWithRedisCache(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisHost = configuration["Redis:Host"] ?? "redis";
        var redisPort = int.TryParse(configuration["Redis:Port"], out var p) ? p : 6379;
        var redisDb = int.TryParse(configuration["Redis:Db"], out var d) ? d : 0;
        var redisPass = configuration["Redis:Password"];

        services
            .AddOcelot(configuration)
            .AddCacheManager(x =>
            {
                x.WithSerializer(typeof(NewtonsoftJsonCacheSerializer))
                    .WithRedisConfiguration("redis", cfg =>
                    {
                        cfg.WithEndpoint(redisHost, redisPort);
                        cfg.WithDatabase(redisDb);

                        if (!string.IsNullOrWhiteSpace(redisPass))
                            cfg.WithPassword(redisPass);
                    })
                    .WithMaxRetries(100)
                    .WithRetryTimeout(50)
                    .WithRedisBackplane("redis")
                    .WithRedisCacheHandle("redis");
            });

        return services;
    }
}