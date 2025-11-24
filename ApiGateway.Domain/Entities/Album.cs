namespace ApiGateway.Domain.Entities;

public class Album : MongoDocument
{
    public string Title { get; set; } = default!;
    public string ArtistId { get; set; } = default!;
    public int Year { get; set; }
}