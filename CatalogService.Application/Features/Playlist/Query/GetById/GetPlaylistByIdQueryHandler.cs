using CatalogService.Application.Abstract;
using CatalogService.Application.Dto_s.PlaylistDto_s;
using ErrorOr;
using Mapster;
using MediatR;
using MongoDB.Bson;
using PlaylistEntity = CatalogService.Domain.Entities.Playlist;

namespace CatalogService.Application.Features.Playlist.Query.GetById;

public class GetPlaylistByIdQueryHandler : IRequestHandler<GetPlaylistByIdQuery, ErrorOr<GetPlaylistDto>>
{
    
    private readonly IMongoReadRepository<PlaylistEntity> _mongoReadRepository;

    public GetPlaylistByIdQueryHandler(IMongoReadRepository<PlaylistEntity> mongoReadRepository)
    {
        _mongoReadRepository = mongoReadRepository;
    }
    
    public async Task<ErrorOr<GetPlaylistDto>> Handle(GetPlaylistByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ObjectId.TryParse(request.Id, out _))
            {
                return Error.Validation(
                    code: "Track.Id.Invalid",
                    description: $"'{request}' is not a valid ObjectId.");
            }

            var entity = await _mongoReadRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                return Error.NotFound(
                    code: "Track.NotFound",
                    description: $"Track with id '{request.Id}' was not found.");
            }

            var dto = entity.Adapt<GetPlaylistDto>();
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