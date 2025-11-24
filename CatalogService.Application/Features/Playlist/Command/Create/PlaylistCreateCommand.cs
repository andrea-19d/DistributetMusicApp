using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Playlist.Command.Create;

public class PlaylistCreateCommand : IRequest<ErrorOr<bool>>
{
    public string UserId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public List<string> TrackIds { get; set; } = new();
}