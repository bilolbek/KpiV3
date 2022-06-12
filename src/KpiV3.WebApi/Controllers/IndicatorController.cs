using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Indicators.Commands;
using KpiV3.Domain.Indicators.Queries;
using KpiV3.WebApi.DataContracts.Indicators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class IndicatorController : ControllerBase
{
    private readonly IMediator _mediator;

    public IndicatorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<IndicatorDto>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAsync([FromQuery] GetIndicatorsRequest request, CancellationToken cancellationToken)
    {
        var indicators = await _mediator.Send(request.ToQuery(), cancellationToken);

        return Ok(indicators.Map(i => new IndicatorDto(i)));
    }

    [HttpGet("{indicatorId:guid}")]
    public async Task<IActionResult> GetAsync(Guid indicatorId, CancellationToken cancellationToken)
    {
        var indicator = await _mediator.Send(new GetIndicatorQuery { IndicatorId = indicatorId }, cancellationToken);

        return Ok(new IndicatorDto(indicator));
    }

    [HttpPost]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(IndicatorDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateIndicatorRequest request)
    {
        var indicator = await _mediator.Send(request.ToCommand());

        return Ok(new IndicatorDto(indicator));
    }

    [HttpPut("{indicatorId:guid}")]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(IndicatorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid indicatorId, [FromBody] UpdateIndicatorRequest request)
    {
        var indicator = await _mediator.Send(request.ToCommand(indicatorId));

        return Ok(new IndicatorDto(indicator));
    }

    [HttpDelete("{indicatorId:guid}")]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid indicatorId)
    {
        await _mediator.Send(new DeleteIndicatorCommand { IndicatorId = indicatorId });

        return Ok();
    }
}
