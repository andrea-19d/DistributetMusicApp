using CatalogService.Application.Dto_s.PlaylistDto_s;
using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Playlist.Command.Update;

public class UpdatePlaylistCommand : IRequest<ErrorOr<bool>>
{
    public string Id { get; set; }
    public UpdatePlaylistDto Dto { get; set; }
}