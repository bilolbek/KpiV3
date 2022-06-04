using KpiV3.Domain.Specialties.Commands;
using KpiV3.Domain.Specialties.Queries;
using KpiV3.WebApi.DataContracts.Specialties;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class SpecialtyController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecialtyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-position/{positionId:guid}")]
    [ProducesResponseType(200, Type = typeof(List<SpecialtyDto>))]
    public async Task<IActionResult> GetAsync(Guid positionId)
    {
        return await _mediator
            .Send(new GetSpecialtiesQuery { PositionId = positionId })
            .MapAsync(specialties => specialties.Select(s => new SpecialtyDto(s)).ToList())
            .MatchAsync(specialties => Ok(specialties), error => error.MapToActionResult());
    }

    [HttpGet("{specialtyId:guid}")]
    [ProducesResponseType(200, Type = typeof(SpecialtyDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid specialtyId)
    {
        return await _mediator
            .Send(new GetSpecialtyQuery { SpecialtyId = specialtyId })
            .MapAsync(specialty => new SpecialtyDto(specialty))
            .MatchAsync(specialty => Ok(specialty), error => error.MapToActionResult());
    }

    [HttpPost]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(SpecialtyDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSpecialtyRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MapAsync(specialty => new SpecialtyDto(specialty))
            .MatchAsync(specialty => Ok(specialty), error => error.MapToActionResult());
    }

    [HttpPut("{specialtyId:guid}")]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(SpecialtyDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid specialtyId, [FromBody] UpdateSpecialtyRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(specialtyId))
            .MapAsync(specialty => new SpecialtyDto(specialty))
            .MatchAsync(specialty => Ok(specialty), error => error.MapToActionResult());
    }

    [HttpDelete("{specialtyId:guid}")]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid specialtyId)
    {
        return await _mediator
            .Send(new DeleteSpecialtyCommand { SpecialtyId = specialtyId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
