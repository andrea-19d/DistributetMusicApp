using CatalogService.Application.Abstract;
using CatalogService.Application.Dto_s;
using CatalogService.Application.Dto_s.TrackDto_s;
using MediatR;
using ErrorOr;
using Mapster;
using MongoDB.Bson;
using TrackEntity = CatalogService.Domain.Entities.Track;


namespace CatalogService.Application.Features.Track.Query.GetById;

public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, ErrorOr<GetTrackDto>>
{
    private readonly IMongoReadRepository<TrackEntity> _readRepository;

    public GetTrackByIdQueryHandler(IMongoReadRepository<TrackEntity> readRepository)
    {
        _readRepository = readRepository;
    }
    
    public async Task<ErrorOr<GetTrackDto>> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ObjectId.TryParse(request.Id, out _))
            {
                return Error.Validation(
                    code: "Track.Id.Invalid",
                    description: $"'{request}' is not a valid ObjectId.");
            }

            var entity = await _readRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                return Error.NotFound(
                    code: "Track.NotFound",
                    description: $"Track with id '{request.Id}' was not found.");
            }

            var dto = entity.Adapt<GetTrackDto>();
            return dto;
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