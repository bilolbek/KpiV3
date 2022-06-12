using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.Queries;
using KpiV3.WebApi.DataContracts.Positions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PositionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PositionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{positionId:guid}")]
    [ProducesResponseType(200, Type = typeof(PositionDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync(Guid positionId, CancellationToken cancellationToken)
    {
        var position = await _mediator.Send(new GetPositionQuery
        {
            PositionId = positionId
        }, cancellationToken);

        return Ok(new PositionDto(position));
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<PositionDto>))]
    public async Task<IActionResult> GetAsync([FromQuery] GetPositionsRequest request, CancellationToken cancellationToken)
    {
        var positions = await _mediator.Send(request.ToQuery(), cancellationToken);

        return Ok(positions.Map(p => new PositionDto(p)));
    }

    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(PositionDto))]
    [ProducesResponseType(400)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePositionRequest request)
    {
        var position = await _mediator.Send(request.ToCommand());

        return Ok(new PositionDto(position));
    }

    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(PositionDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpPut("{positionId:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid positionId, [FromBody] UpdatePositionRequest request)
    {
        var position = await _mediator.Send(request.ToCommand(positionId));

        return Ok(new PositionDto(position));
    }

    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [HttpDelete("{positionId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid positionId)
    {
        await _mediator.Send(new DeletePositionCommand { PositionId = positionId });
        return Ok();
    }
}
