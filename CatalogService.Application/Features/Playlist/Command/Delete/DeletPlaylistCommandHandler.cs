using CatalogService.Application.Abstract;
using ErrorOr;
using MediatR;
using MongoDB.Bson;
using PlaylistEntity = CatalogService.Domain.Entities.Playlist;

namespace CatalogService.Application.Features.Playlist.Command.Delete;

public class DeletPlaylistCommandHandler : IRequestHandler<DeletePlaylistCommand, ErrorOr<bool>>
{
    private readonly IMongoWriteRepository<PlaylistEntity>  _mongoWriteRepository;
    
    public DeletPlaylistCommandHandler(IMongoWriteRepository<PlaylistEntity> mongoWriteRepository)
    {
        _mongoWriteRepository = mongoWriteRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ObjectId.TryParse(request.Id, out _))
            {
                return Error.Validation(
                    code: "Track.Id.Invalid",
                    description: $"'{request}' is not a valid ObjectId.");
            }

            await _mongoWriteRepository.DeleteAsync(request.Id);
            return true;
            
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