namespace CatalogService.Domain.Entities;

public class Track : MongoDocument
{
    public string Title { get; set; } = default!;
    public string AlbumId { get; set; } = default!;
    public string ArtistId { get; set; } = default!;
    public int DurationSeconds { get; set; }
    public DateTime ReleaseDate { get; set; }
    
    // path-ul către fișierul audio în storage (nu în API)
    public string AudioPath { get; set; } = default!;
}