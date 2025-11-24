using CatalogService.Application.Dto_s;
using CatalogService.Application.Dto_s.TrackDto_s;
using MediatR;
using ErrorOr;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Application.Features.Track.Query.GetById;

public class GetTrackByIdQuery : IRequest<ErrorOr<GetTrackDto>>
{
    public required string Id { get; set; }
} 