using CatalogService.Application.Dto_s.AlbumDto_s;
using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Album.Query.GetById;

public class GetAlbumByIdQuery : IRequest<ErrorOr<GetAlbumDto>>
{
    public string Id { get; set; }
}