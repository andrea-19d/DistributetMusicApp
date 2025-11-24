using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Album.Command.Delete;

public class DeleteAlbumCommand : IRequest<ErrorOr<bool>>
{
    public string Id {get;set;}
}