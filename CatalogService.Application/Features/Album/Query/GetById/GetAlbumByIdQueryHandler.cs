using CatalogService.Application.Abstract;
using CatalogService.Application.Dto_s.AlbumDto_s;
using CatalogService.Application.Dto_s.PlaylistDto_s;
using ErrorOr;
using Mapster;
using MediatR;
using MongoDB.Bson;
using AlbumEntity = CatalogService.Domain.Entities.Album;

namespace CatalogService.Application.Features.Album.Query.GetById;

public class GetAlbumByIdQueryHandler : IRequestHandler<GetAlbumByIdQuery, ErrorOr<GetAlbumDto>>
{
    private readonly IMongoReadRepository<AlbumEntity> _mongoReadRepository;

    public GetAlbumByIdQueryHandler(IMongoReadRepository<AlbumEntity> mongoReadRepository)
    {
        _mongoReadRepository = mongoReadRepository;
    }
    
    public async Task<ErrorOr<GetAlbumDto>> Handle(GetAlbumByIdQuery request, CancellationToken cancellationToken)
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

            var dto = entity.Adapt<GetAlbumDto>();
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