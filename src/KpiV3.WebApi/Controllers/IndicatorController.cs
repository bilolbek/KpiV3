using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Indicators.Commands;
using KpiV3.Domain.Indicators.Queries;
using KpiV3.WebApi.DataContracts.Indicators;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize(Policy = "RootOnly")]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("3.0")]
public class IndicatorController : ControllerBase
{
    private readonly IMediator _mediator;

    public IndicatorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<IndicatorDto>))]
    public async Task<IActionResult> GetAsync([FromQuery] GetIndicatorsRequest request)
    {
        return await _mediator
            .Send(request.ToQuery())
            .MapAsync(indicators => indicators.Map(indicator => new IndicatorDto(indicator)))
            .MatchAsync(indicators => Ok(indicators), error => error.MapToActionResult());
    }

    [HttpGet("{indicatorId:guid}")]
    [ProducesResponseType(200, Type = typeof(IndicatorDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid indicatorId)
    {
        return await _mediator
            .Send(new GetIndicatorQuery { IndicatorId = indicatorId })
            .MapAsync(indicator => new IndicatorDto(indicator))
            .MatchAsync(indicator => Ok(indicator), error => error.MapToActionResult());
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(IndicatorDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateIndicatorRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MapAsync(indicator => new IndicatorDto(indicator))
            .MatchAsync(indicator => Ok(indicator), error => error.MapToActionResult());
    }

    [HttpPut("{indicatorId:guid}")]
    [ProducesResponseType(200, Type = typeof(IndicatorDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid indicatorId, [FromBody] UpdateIndicatorRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(indicatorId))
            .MapAsync(indicator => new IndicatorDto(indicator))
            .MatchAsync(indicator => Ok(indicator), error => error.MapToActionResult());
    }

    [HttpDelete("{indicatorId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid indicatorId)
    {
        return await _mediator
            .Send(new DeleteIndicatorCommand { IndicatorId = indicatorId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
