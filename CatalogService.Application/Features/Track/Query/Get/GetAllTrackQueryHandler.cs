using CatalogService.Application.Abstract;
using CatalogService.Application.Dto_s.TrackDto_s;
using MediatR;
using ErrorOr;
using Mapster;

using TrackEntity = CatalogService.Domain.Entities.Track;

namespace CatalogService.Application.Features.Track.Query.Get;

public class GetAllTrackQueryHandler : IRequestHandler<GetAllTrackQuery, ErrorOr<List<GetTrackDto>>>
{
    private readonly IMongoReadRepository<TrackEntity> _trackRepository;

    public GetAllTrackQueryHandler(IMongoReadRepository<TrackEntity> trackRepository)
    {
        _trackRepository = trackRepository;
    }

    public async Task<ErrorOr<List<GetTrackDto>>> Handle(GetAllTrackQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _trackRepository.GetAllAsync();         
            var dtos = entities.Adapt<List<GetTrackDto>>();               

            return dtos; 
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error.Failure(
                code: "Track.Get.Failed",
                description: ex.Message);
        }
    }
}