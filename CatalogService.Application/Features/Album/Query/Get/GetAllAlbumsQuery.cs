using CatalogService.Application.Dto_s.AlbumDto_s;
using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Album.Query.Get;

public class GetAllAlbumsQuery : IRequest<ErrorOr<List<GetAlbumDto>>>
{
    
}