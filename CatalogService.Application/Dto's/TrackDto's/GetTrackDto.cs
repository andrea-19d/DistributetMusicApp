using CatalogService.Domain.Entities;

namespace CatalogService.Application.Dto_s.TrackDto_s;

public class GetTrackDto : MongoDocument
{
    public string? Title { get; set; } 
    public string? AlbumId { get; set; }
    public string? ArtistId { get; set; } 
    public int DurationSeconds { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? AudioPath { get; set; }
}