using ErrorOr;
using Mapster;
using MediatR;
using CatalogService.Application.Abstract;
using CatalogService.Application.Dto_s.AlbumDto_s;
using AlbumEntity = CatalogService.Domain.Entities.Album;

namespace CatalogService.Application.Features.Album.Query.Get;

public class GetAllAlbumsQueryHandler : IRequestHandler<GetAllAlbumsQuery, ErrorOr<List<GetAlbumDto>>>
{
    private readonly IMongoReadRepository<AlbumEntity>  _albumRepository;

    public GetAllAlbumsQueryHandler(IMongoReadRepository<AlbumEntity> albumRepository)
    {
        _albumRepository = albumRepository;
    }
    
    public async Task<ErrorOr<List<GetAlbumDto>>> Handle(GetAllAlbumsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _albumRepository.GetAllAsync();         
            var dtos = entities.Adapt<List<GetAlbumDto>>();               

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