using CatalogService.Application.Abstract;
using CatalogService.Application.Dto_s.PlaylistDto_s;
using ErrorOr;
using Mapster;
using MediatR;
using PlaylistEntity = CatalogService.Domain.Entities.Playlist;

namespace CatalogService.Application.Features.Playlist.Query.Get;

public class GetAllPlaylistQueryHandler : IRequestHandler<GetAllPlaylistQuery, ErrorOr<List<GetPlaylistDto>>>
{
    
    private readonly IMongoReadRepository<PlaylistEntity> _mongoReadRepository;

    public GetAllPlaylistQueryHandler(IMongoReadRepository<PlaylistEntity> mongoReadRepository)
    {
        _mongoReadRepository = mongoReadRepository;
    }

    public async Task<ErrorOr<List<GetPlaylistDto>>> Handle(GetAllPlaylistQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _mongoReadRepository.GetAllAsync();         
            var dtos = entities.Adapt<List<GetPlaylistDto>>();               

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