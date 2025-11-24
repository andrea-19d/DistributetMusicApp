using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Track.Command.Delete;

public class DeleteTrackCommand : IRequest<ErrorOr<bool>>
{
    public string TrackId { get; set; }
}