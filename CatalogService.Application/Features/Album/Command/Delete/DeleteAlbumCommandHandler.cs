using CatalogService.Application.Abstract;
using ErrorOr;
using MediatR;
using MongoDB.Bson;
using AlbumEntity = CatalogService.Domain.Entities.Album;

namespace CatalogService.Application.Features.Album.Command.Delete;

public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand, ErrorOr<bool>>
{
    private readonly IMongoWriteRepository<AlbumEntity> _writeRepository;

    public DeleteAlbumCommandHandler(IMongoWriteRepository<AlbumEntity> writeRepository)
    {
        _writeRepository = writeRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ObjectId.TryParse(request.Id, out _))
            {
                return Error.Validation(
                    code: "Track.Id.Invalid",
                    description: $"'{request}' is not a valid ObjectId.");
            }

            await _writeRepository.DeleteAsync(request.Id);
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