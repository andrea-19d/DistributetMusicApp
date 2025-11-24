namespace CatalogService.Application.Dto_s.TrackDto_s;

public class UpdateTrackDto
{
    public string? Title { get; set; }
    public string? AlbumId { get; set; }
    public string? ArtistId { get; set; }
    public int? DurationSeconds { get; set; }
}