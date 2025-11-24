using CatalogService.Application.Dto_s.AlbumDto_s;
using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Album.Command.Update;

public class UpdateAlbumCommand : IRequest<ErrorOr<bool>>
{
    public string Id { get; set; }
    public UpdateAlbumDto? Album { get; set; }
}