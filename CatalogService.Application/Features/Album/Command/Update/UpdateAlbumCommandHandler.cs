using CatalogService.Application.Abstract;
using ErrorOr;
using Mapster;
using MediatR;
using MongoDB.Bson;
using AlbumEntity = CatalogService.Domain.Entities.Album;


namespace CatalogService.Application.Features.Album.Command.Update;

public class UpdateAlbumCommandHandler : IRequestHandler<UpdateAlbumCommand, ErrorOr<bool>>
{
    private readonly IMongoWriteRepository<AlbumEntity> _writeRepository;
    private readonly IMongoReadRepository<AlbumEntity> _readRepository;

    public UpdateAlbumCommandHandler(IMongoWriteRepository<AlbumEntity> writeRepository,  IMongoReadRepository<AlbumEntity> readRepository)
    {
        _writeRepository = writeRepository;
        _readRepository = readRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Id) || !ObjectId.TryParse(request.Id, out _))
            {
                return Error.Validation(
                    code: "Track.Id.Invalid",
                    description: $"'{request.Id}' is not a valid ObjectId.");
            }

            var existing = await _readRepository.GetByIdAsync(request.Id);
            if (existing is null)
            {
                return Error.NotFound(
                    code: "Track.NotFound",
                    description: $"Track with id '{request.Id}' not found.");
            }
            
            request.Album.Adapt(existing);
            
            await _writeRepository.UpdateAsync(existing);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Error.Failure(
                code: "Track.Update.Failed",
                description: ex.Message);
        }
    }
}