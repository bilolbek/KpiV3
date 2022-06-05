using KpiV3.Domain.PeriodParts.Commands;
using KpiV3.Domain.PeriodParts.Queries;
using KpiV3.WebApi.DataContracts.PeriodParts;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize(Policy = "RootOnly")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class PeriodPartController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeriodPartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-period/{periodId:guid}")]
    [ProducesResponseType(200, Type = typeof(List<PeriodPartDto>))]
    public async Task<IActionResult> GetByPeriodIdAsync(Guid periodId)
    {
        return await _mediator
            .Send(new GetPeriodPartsQuery { PeriodId = periodId })
            .MapAsync(parts => parts.Select(part => new PeriodPartDto(part)).ToList())
            .MatchAsync(parts => Ok(parts), error => error.MapToActionResult());
    }

    [HttpGet("{partId:guid}")]
    [ProducesResponseType(200, Type = typeof(PeriodPartDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid partId)
    {
        return await _mediator
            .Send(new GetPeriodPartQuery { PartId = partId })
            .MapAsync(part => new PeriodPartDto(part))
            .MatchAsync(part => Ok(part), error => error.MapToActionResult());
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(PeriodPartDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePeriodPartRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MapAsync(part => new PeriodPartDto(part))
            .MatchAsync(part => Ok(part), error => error.MapToActionResult());
    }

    [HttpPut("{partId:guid}")]
    [ProducesResponseType(200, Type = typeof(PeriodPartDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid partId, [FromBody] UpdatePeriodPartRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(partId))
            .MapAsync(part => new PeriodPartDto(part))
            .MatchAsync(part => Ok(part), error => error.MapToActionResult());
    }

    [HttpDelete("{partId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid partId)
    {
        return await _mediator
            .Send(new DeletePeriodPartCommand { PartId = partId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
