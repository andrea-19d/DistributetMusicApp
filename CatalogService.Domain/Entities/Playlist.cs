namespace CatalogService.Domain.Entities;

public class Playlist : MongoDocument
{
    public string UserId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public List<string> TrackIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}