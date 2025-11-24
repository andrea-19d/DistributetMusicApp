using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Playlist.Command.Delete;

public class DeletePlaylistCommand : IRequest<ErrorOr<bool>>
{
    public string Id { get; set; }
}