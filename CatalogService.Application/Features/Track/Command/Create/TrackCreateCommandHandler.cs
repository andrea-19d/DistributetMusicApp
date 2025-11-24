using CatalogService.Application.Abstract;
using ErrorOr;
using Mapster;
using MediatR;
using TrackEntity = CatalogService.Domain.Entities.Track;

namespace CatalogService.Application.Features.Track.Command.Create;

public class TrackCreateCommandHandler 
    : IRequestHandler<TrackCreateCommand, ErrorOr<bool>>
{
    private readonly IMongoWriteRepository<TrackEntity>  _trackRepository;

    public TrackCreateCommandHandler(IMongoWriteRepository<TrackEntity> trackRepository)
    {
        _trackRepository = trackRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(TrackCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = request.Adapt<TrackEntity>();

            await _trackRepository.InsertAsync(entity);

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