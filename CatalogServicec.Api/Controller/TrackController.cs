using CatalogService.Application.Dto_s;
using CatalogService.Application.Dto_s.TrackDto_s;
using CatalogService.Application.Features.Track.Command.Create;
using CatalogService.Application.Features.Track.Command.Delete;
using CatalogService.Application.Features.Track.Command.Update;
using CatalogService.Application.Features.Track.Query.Get;
using CatalogService.Application.Features.Track.Query.GetById;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServicec.Api.Controller;

[ApiController]
[Route("catalog-service/[controller]")]
public class TrackController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrackController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateTrack([FromBody] TrackCreateCommand request)
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
    public async Task<ActionResult<List<GetTrackDto>>> GetAll()
    {
        var query = new GetAllTrackQuery();

        ErrorOr<List<GetTrackDto>> result = await _mediator.Send(query);

        if (result.IsError)
        {
            var error = result.FirstError;
            return Problem(title: error.Code, detail: error.Description, statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetTrackDto>> GetTrack([FromRoute] string id)
    {
        var query = new GetTrackByIdQuery { Id = id };

        var result = await _mediator.Send(query);

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
    public async Task<IActionResult> Update(string id, [FromBody] UpdateTrackDto dto)
    {
        var cmd = new UpdateTrackCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(cmd);

        if (result.IsError)
            return Problem(
                title: result.FirstError.Code, 
                detail: result.FirstError.Description, 
                statusCode: StatusCodes.Status400BadRequest);

        return Ok(result.Value);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(DeleteTrackCommand request)
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