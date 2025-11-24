using CatalogService.Application.Dto_s.PlaylistDto_s;
using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Playlist.Query.Get;

public class GetAllPlaylistQuery : IRequest<ErrorOr<List<GetPlaylistDto>>>
{
    
}