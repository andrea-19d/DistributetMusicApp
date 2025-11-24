using CatalogService.Application.Dto_s;
using CatalogService.Application.Dto_s.TrackDto_s;
using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Track.Command.Update;

public class UpdateTrackCommand : IRequest<ErrorOr<bool>>
{
    public required string Id { get; set; }
    public required UpdateTrackDto Dto { get; set; }
}