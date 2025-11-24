using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Album.Command.Create;

public class CreateAlbumCommand : IRequest<ErrorOr<bool>>
{
    public string Title { get; set; } = default!;
    public string ArtistId { get; set; } = default!;
}