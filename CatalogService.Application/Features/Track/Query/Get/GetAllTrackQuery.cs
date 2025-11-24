using CatalogService.Application.Dto_s;
using CatalogService.Application.Dto_s.TrackDto_s;
using MediatR;
using ErrorOr; 

namespace CatalogService.Application.Features.Track.Query.Get;

public class GetAllTrackQuery : IRequest<ErrorOr<List<GetTrackDto>>>
{
    
}