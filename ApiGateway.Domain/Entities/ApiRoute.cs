using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiGateway.Domain.Entities;

public class ApiRoute : MongoDocument
{
    public string Name { get; set; } = default!;
    public string UpstreamPath { get; set; } = default!;      // e.g. "/api/products"
    public string DownstreamUrl { get; set; } = default!;     // e.g. "http://catalog.api:8080/api/products"
    public string Method { get; set; } = "GET";               // GET/POST/etc
}