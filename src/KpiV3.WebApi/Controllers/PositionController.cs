using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.Queries;
using KpiV3.WebApi.DataContracts.Positions;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize(Policy = "RootOnly")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class PositionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PositionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<PositionDto>))]
    public async Task<IActionResult> GetAsync([FromQuery] GetPositionsRequest request)
    {
        return await _mediator
            .Send(request.ToQuery())
            .MapAsync(positions => positions.Map(p => new PositionDto(p)))
            .MatchAsync(positions => Ok(positions), error => error.MapToActionResult());
    }

    [HttpGet("{positionId:guid}")]
    [ProducesResponseType(200, Type = typeof(PositionDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid positionId)
    {
        return await _mediator
            .Send(new GetPositionQuery { PositionId = positionId })
            .MapAsync(position => new PositionDto(position))
            .MatchAsync(position => Ok(position), error => error.MapToActionResult());
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(PositionDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePositionRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MapAsync(position => new PositionDto(position))
            .MatchAsync(position => Ok(position), error => error.MapToActionResult());
    }

    [HttpDelete("{positionId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid positionId)
    {
        return await _mediator
            .Send(new DeletePositionCommand { PositionId = positionId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }

    [HttpPut("{positionId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateAsync(Guid positionId, UpdatePositionRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(positionId))
            .MapAsync(position => new PositionDto(position))
            .MatchAsync(position => Ok(position), error => error.MapToActionResult());
    }
}
