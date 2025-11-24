using CatalogService.Application.Abstract;
using CatalogService.Application.Dto_s;
using ErrorOr;
using Mapster;
using MediatR;
using MongoDB.Bson;
using TrackEntity = CatalogService.Domain.Entities.Track;

namespace CatalogService.Application.Features.Track.Command.Delete;

public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand, ErrorOr<bool>>
{
    private readonly IMongoWriteRepository<TrackEntity> _writeRepository;

    public DeleteTrackCommandHandler(IMongoWriteRepository<TrackEntity> writeRepository)
    {
        _writeRepository = writeRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ObjectId.TryParse(request.TrackId, out _))
            {
                return Error.Validation(
                    code: "Track.Id.Invalid",
                    description: $"'{request}' is not a valid ObjectId.");
            }

            await _writeRepository.DeleteAsync(request.TrackId);
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