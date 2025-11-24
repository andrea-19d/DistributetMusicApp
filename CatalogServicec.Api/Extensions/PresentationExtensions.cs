using CatalogService.Application.Extension;
using Mapster;
using Microsoft.OpenApi;

namespace CatalogServicec.Api.Extensions;

public static class PresentationExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddDataProtection();
        services.AddSwagger();
        
        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Catalog Service API",
                Version = "v1",
                Description = "Catalog Service API",
            });
        });

        return services;
    }
}