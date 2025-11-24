using ErrorOr;
using MediatR;

namespace CatalogService.Application.Features.Track.Command.Create;

public class TrackCreateCommand : IRequest<ErrorOr<bool>>
{
    public string? Title { get; set; } 
    public string? AlbumId { get; set; }
    public string? ArtistId { get; set; } 
    public int DurationSeconds { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? AudioPath { get; set; }
}