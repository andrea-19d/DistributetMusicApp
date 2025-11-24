using CatalogService.Application.Abstract;
using ErrorOr;
using Mapster;
using MediatR;
using AlbumEntity = CatalogService.Domain.Entities.Album;

namespace CatalogService.Application.Features.Album.Command.Create;

public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, ErrorOr<bool>>
{
    private readonly IMongoWriteRepository<AlbumEntity>  _writeRepository;

    public CreateAlbumCommandHandler(IMongoWriteRepository<AlbumEntity> writeRepository)
    {
        _writeRepository = writeRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = request.Adapt<AlbumEntity>();
            entity.Year = DateTime.Now.Year;
            await _writeRepository.InsertAsync(entity);

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