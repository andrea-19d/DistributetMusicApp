using ApiGateway.Application.Abstract;
using ApiGateway.Application.Dto_s;
using ApiGateway.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ApiRoutesController : ControllerBase
{
    private readonly IMongoRepository<ApiRoute> _mongoRepository;

    public ApiRoutesController(IMongoRepository<ApiRoute> mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<List<ApiRouteResponse>>> Create([FromBody] CreateApiRouteRequest request)
    {
        Console.WriteLine("created thnsd");
        
        var entity = new ApiRoute
        {
            Name = request.Name,
            UpstreamPath = request.UpstreamPath,
            DownstreamUrl = request.DownstreamUrl,
            Method = request.Method
        };

        _mongoRepository.InsertRecords(entity);

        var records = await _mongoRepository.GetAllRecords();

        return Ok(HttpContext.Response.StatusCode);
    }


    // GET api/apiroutes
    // List all routes
    [HttpGet]
    public async Task<ActionResult<List<ApiRouteResponse>>> GetAll()
    {
        var records = await _mongoRepository.GetAllRecords();
        
        var response = records.Select(r => new ApiRouteResponse
        {
            Id = r.Id.ToString(),
            Name = r.Name,
            UpstreamPath = r.UpstreamPath,
            DownstreamUrl = r.DownstreamUrl,
            Method = r.Method,
            LastUpdated = r.LastUpdated
        }).ToList();

        return Ok(response);
    }
}
