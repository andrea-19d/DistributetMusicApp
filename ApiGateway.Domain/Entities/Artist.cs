namespace ApiGateway.Domain.Entities;

public class Artist : MongoDocument
{
    public string Name { get; set; } = default!;
}