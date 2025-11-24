using CatalogService.Application.Dto_s.AlbumDto_s;
using CatalogService.Application.Features.Album.Command.Create;
using CatalogService.Application.Features.Album.Command.Delete;
using CatalogService.Application.Features.Album.Command.Update;
using CatalogService.Application.Features.Album.Query.Get;
using CatalogService.Application.Features.Album.Query.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServicec.Api.Controller;

[ApiController]
[Route("catalog-service/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly IMediator _mediator;

    public AlbumController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAlbum([FromBody] CreateAlbumCommand createAlbumCommand)
    {
        var result = await _mediator.Send(createAlbumCommand);
        if (result.IsError)
        {
            var error = result.FirstError;
            return Problem(title: error.Code, detail: error.Description, statusCode: StatusCodes.Status400BadRequest);
        }
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<GetAlbumDto>>> GetAll()
    {
        var query = new GetAllAlbumsQuery();

        var result = await _mediator.Send(query);

        if (result.IsError)
        {
            var error = result.FirstError;
            return Problem(title: error.Code, detail: error.Description, statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetAlbumDto>> GetTrack([FromRoute] GetAlbumByIdQuery request)
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
    public async Task<IActionResult> Update(UpdateAlbumCommand request)
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
    public async Task<IActionResult> Delete(DeleteAlbumCommand request)
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