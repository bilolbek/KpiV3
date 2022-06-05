using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Periods.Commands;
using KpiV3.Domain.Periods.Queries;
using KpiV3.WebApi.DataContracts.Periods;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize(Policy = "RootOnly")]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("3.0")]
public class PeriodController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeriodController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<PeriodDto>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAsync([FromQuery] GetPeriodsRequest request)
    {
        return await _mediator
            .Send(request.ToQuery())
            .MapAsync(periods => periods.Map(period => new PeriodDto(period)))
            .MatchAsync(periods => Ok(periods), error => error.MapToActionResult());
    }

    [HttpGet("{periodId:guid}")]
    [ProducesResponseType(200, Type = typeof(PeriodDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid periodId)
    {
        return await _mediator
            .Send(new GetPeriodQuery { PeriodId = periodId })
            .MapAsync(period => new PeriodDto(period))
            .MatchAsync(period => Ok(period), error => error.MapToActionResult());
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(Page<PeriodDto>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePeriodRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MapAsync(period => new PeriodDto(period))
            .MatchAsync(period => Ok(period), error => error.MapToActionResult());
    }

    [HttpPut("{periodId:guid}")]
    [ProducesResponseType(200, Type = typeof(Page<PeriodDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid periodId, [FromBody] UpdatePeriodRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(periodId))
            .MapAsync(period => new PeriodDto(period))
            .MatchAsync(period => Ok(period), error => error.MapToActionResult());
    }

    [HttpDelete("{periodId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid periodId)
    {
        return await _mediator
            .Send(new DeletePeriodCommand { PeriodId = periodId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
