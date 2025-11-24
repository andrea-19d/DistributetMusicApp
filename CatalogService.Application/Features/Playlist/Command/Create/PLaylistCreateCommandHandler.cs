using CatalogService.Application.Abstract;
using ErrorOr;
using Mapster;
using MediatR;
using PlaylistEntity = CatalogService.Domain.Entities.Playlist;


namespace CatalogService.Application.Features.Playlist.Command.Create;

public class PLaylistCreateCommandHandler : IRequestHandler<PlaylistCreateCommand, ErrorOr<bool>>
{
    private readonly IMongoWriteRepository<PlaylistEntity>  _mongoWriteRepository;

    public PLaylistCreateCommandHandler(IMongoWriteRepository<PlaylistEntity> mongoWriteRepository)
    {
        _mongoWriteRepository = mongoWriteRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(PlaylistCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = request.Adapt<PlaylistEntity>();
            entity.CreatedAt = DateTime.Now;
            await _mongoWriteRepository.InsertAsync(entity);

            return true; 
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Error.Failure(
                code: "Track.Create.Failed",
                description: ex.Message);
        }
    }
}