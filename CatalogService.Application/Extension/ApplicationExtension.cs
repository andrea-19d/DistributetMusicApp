using CatalogService.Application.Features.Track.Command.Create;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Application.Extension;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterMapsterConfiguration();
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(TrackCreateCommand).Assembly);
        });
        
        return services;
    }
    
    private static void RegisterMapsterConfiguration()
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ApplicationExtension).Assembly);
    }
    
}