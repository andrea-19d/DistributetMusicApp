using Microsoft.OpenApi;

namespace ApiGateway.Api.Extensions;

public static class PresentationExtensions
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddControllers(o => o.RespectBrowserAcceptHeader = true)
            .AddXmlSerializerFormatters()
            .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

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
                Title = "API Gateway",
                Version = "v1",
                Description = "API Gateway for microservices"
            });
        });

        return services;
    }
}