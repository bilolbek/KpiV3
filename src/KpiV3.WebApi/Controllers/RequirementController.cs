using KpiV3.Domain.Requirements.Commands;
using KpiV3.Domain.Requirements.Queries;
using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.WebApi.DataContracts.Requirements;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("3.0")]
public class RequirementController : ControllerBase
{
    private readonly IMediator _mediator;

    public RequirementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "RootOnly")]
    [HttpGet("for-employee/{employeeId:guid}/{period:guid}")]
    [ProducesResponseType(200, Type = typeof(List<RequirementDto>))]
    public async Task<IActionResult> GetRequriementsOfEmployeeAsync(Guid employeeId, Guid periodId)
    {
        return await _mediator
            .Send(new GetRequirementsOfEmployeeQuery { PeriodId = periodId, EmployeeId = employeeId })
            .MapAsync(r => r.Select(r => new RequirementDto(r)).ToList())
            .MatchAsync(r => Ok(r), error => error.MapToActionResult());
    }

    [HttpGet("{specialtyId:guid}/{periodId:guid}")]
    [ProducesResponseType(200, Type = typeof(List<RequirementDto>))]
    public async Task<IActionResult> GetByPeriodIdAsync(Guid specialtyId, Guid periodId)
    {
        return await _mediator
            .Send(new GetRequirementsQuery { PeriodId = periodId, SpecialtyId = specialtyId })
            .MapAsync(r => r.Select(r => new RequirementDto(r)).ToList())
            .MatchAsync(r => Ok(r), error => error.MapToActionResult());
    }

    [HttpGet("{requirementId:guid}")]
    [ProducesResponseType(200, Type = typeof(RequirementDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid requirementId)
    {
        return await _mediator
            .Send(new GetRequirementQuery { RequirementId = requirementId })
            .MapAsync(r => new RequirementDto(r))
            .MatchAsync(r => Ok(r), error => error.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(RequirementDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRequirementRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MapAsync(r => new RequirementDto(r))
            .MatchAsync(r => Ok(r), error => error.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPut("{requirementId:guid}")]
    [ProducesResponseType(200, Type = typeof(RequirementDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid requirementId, [FromBody] UpdateRequirementRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(requirementId))
            .MapAsync(r => new RequirementDto(r))
            .MatchAsync(r => Ok(r), error => error.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpDelete("{requirementId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid requirementId)
    {
        return await _mediator
            .Send(new DeleteRequirementCommand { RequirementId = requirementId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
