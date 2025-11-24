namespace ApiGateway.Application.Dto_s;

public class ApiRouteResponse
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string UpstreamPath { get; set; } = default!;
    public string DownstreamUrl { get; set; } = default!;
    public string Method { get; set; } = default!;
    public DateTime LastUpdated { get; set; }
}