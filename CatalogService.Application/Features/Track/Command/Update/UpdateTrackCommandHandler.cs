using CatalogService.Application.Abstract;
using ErrorOr;
using Mapster;
using MediatR;
using MongoDB.Bson;
using TrackEntity = CatalogService.Domain.Entities.Track;

namespace CatalogService.Application.Features.Track.Command.Update;

public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand, ErrorOr<bool>>
{
    private readonly IMongoReadRepository<TrackEntity> _readRepo;
    private readonly IMongoWriteRepository<TrackEntity> _writeRepo;

    public UpdateTrackCommandHandler(
        IMongoReadRepository<TrackEntity> readRepo,
        IMongoWriteRepository<TrackEntity> writeRepo)
    {
        _readRepo = readRepo;
        _writeRepo = writeRepo;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Id) || !ObjectId.TryParse(request.Id, out _))
            {
                return Error.Validation(
                    code: "Track.Id.Invalid",
                    description: $"'{request.Id}' is not a valid ObjectId.");
            }

            var existing = await _readRepo.GetByIdAsync(request.Id);
            if (existing is null)
            {
                return Error.NotFound(
                    code: "Track.NotFound",
                    description: $"Track with id '{request.Id}' not found.");
            }
            
            request.Dto.Adapt(existing);
            
            await _writeRepo.UpdateAsync(existing);

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