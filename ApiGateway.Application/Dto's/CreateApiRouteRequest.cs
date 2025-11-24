namespace ApiGateway.Application.Dto_s;

public class CreateApiRouteRequest
{
    public string Name { get; set; } = default!;
    public string UpstreamPath { get; set; } = default!;
    public string DownstreamUrl { get; set; } = default!;
    public string Method { get; set; } = "GET";
}