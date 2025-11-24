namespace CatalogService.Application.Dto_s.PlaylistDto_s;

public class GetPlaylistDto
{
    public string UserId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public List<string> TrackIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdated { get; set; }
}