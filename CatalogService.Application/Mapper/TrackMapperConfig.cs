using CatalogService.Application.Dto_s;
using CatalogService.Application.Dto_s.TrackDto_s;
using CatalogService.Application.Features.Track.Command.Create;
using CatalogService.Domain.Entities;
using Mapster;

namespace CatalogService.Application.Mapper;

public static class TrackMapperConfig
{
    public static void Register()
    {
        TypeAdapterConfig<TrackCreateCommand, Track>.NewConfig();
        TypeAdapterConfig<Track, TrackCreateCommand>.NewConfig();
        TypeAdapterConfig<UpdateTrackDto, Track>
            .NewConfig()
            .IgnoreNullValues(true);
    }
}