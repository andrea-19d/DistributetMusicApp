namespace CatalogService.Application.Dto_s.PlaylistDto_s;

public class UpdatePlaylistDto
{
    public string UserId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public List<string> TrackIds { get; set; } = new();
}