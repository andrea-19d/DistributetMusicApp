using CatalogService.Application.Abstract;
using ErrorOr;
using Mapster;
using MediatR;
using MongoDB.Bson;
using PlaylistEntity = CatalogService.Domain.Entities.Playlist;

namespace CatalogService.Application.Features.Playlist.Command.Update;

public class UpdatePlaylistCommandHandler : IRequestHandler<UpdatePlaylistCommand, ErrorOr<bool>>
{
    private readonly IMongoWriteRepository<PlaylistEntity> _mongoWriteRepository;
    private readonly IMongoReadRepository<PlaylistEntity> _mongoReadRepository;

    public UpdatePlaylistCommandHandler(IMongoWriteRepository<PlaylistEntity> mongoWriteRepository,
        IMongoReadRepository<PlaylistEntity> mongoReadRepository)
    {
        _mongoWriteRepository = mongoWriteRepository;
        _mongoReadRepository = mongoReadRepository;
    }

    public async Task<ErrorOr<bool>> Handle(UpdatePlaylistCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Id) || !ObjectId.TryParse(request.Id, out _))
            {
                return Error.Validation(
                    code: "Track.Id.Invalid",
                    description: $"'{request.Id}' is not a valid ObjectId.");
            }

            var existing = await _mongoReadRepository.GetByIdAsync(request.Id);
            if (existing is null)
            {
                return Error.NotFound(
                    code: "Track.NotFound",
                    description: $"Track with id '{request.Id}' not found.");
            }
            request.Dto.Adapt(existing);
            
            await _mongoWriteRepository.UpdateAsync(existing);

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