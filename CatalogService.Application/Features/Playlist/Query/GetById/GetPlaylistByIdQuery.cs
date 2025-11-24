using CatalogService.Application.Dto_s.PlaylistDto_s;
using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Playlist.Query.GetById;

public class GetPlaylistByIdQuery : IRequest<ErrorOr<GetPlaylistDto>>
{
    public string Id { get; set; }
}