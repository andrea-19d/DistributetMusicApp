using CatalogService.Application.Dto_s.PlaylistDto_s;
using CatalogService.Application.Features.Playlist.Command.Create;
using CatalogService.Application.Features.Playlist.Command.Delete;
using CatalogService.Application.Features.Playlist.Command.Update;
using CatalogService.Application.Features.Playlist.Query.Get;
using CatalogService.Application.Features.Playlist.Query.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServicec.Api.Controller;

[ApiController]
[Route("catalog-service/[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlaylistController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateTrack([FromBody] PlaylistCreateCommand request)
    {
        var result = await _mediator.Send(request);
        
        if (result.IsError)
        {
            var error = result.FirstError;
            return Problem(title: error.Code, detail: error.Description, statusCode: StatusCodes.Status400BadRequest);
        }

        return StatusCode(StatusCodes.Status201Created);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<GetPlaylistDto>>> GetAll()
    {
        var query = new GetAllPlaylistQuery();

        var result = await _mediator.Send(query);

        if (result.IsError)
        {
            var error = result.FirstError;
            return Problem(title: error.Code, detail: error.Description, statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPlaylistDto>> GetTrack([FromRoute] GetPlaylistByIdQuery request)
    {
        var result = await _mediator.Send(request);

        if (result.IsError)
        {
            var error = result.FirstError;
            return Problem(
                title: error.Code,
                detail: error.Description,
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok(result.Value);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(UpdatePlaylistCommand request)
    {
        // var cmd = new UpdatePlaylistCommand {  = id, Dto = dto };
        var result = await _mediator.Send(request);

        if (result.IsError)
            return Problem(
                title: result.FirstError.Code, 
                detail: result.FirstError.Description, 
                statusCode: StatusCodes.Status400BadRequest);

        return Ok(result.Value);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(DeletePlaylistCommand request)
    {
        var result = await _mediator.Send(request);

        if (result.IsError)
            return Problem(
                title: result.FirstError.Code, 
                detail: result.FirstError.Description, 
                statusCode: StatusCodes.Status400BadRequest);

        return Ok(result.Value);
    }
}