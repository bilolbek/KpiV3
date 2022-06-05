using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Grades.Queries;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.DataContracts.Grades;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class GradeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEmployeeAccessor _employeeAccessor;

    public GradeController(
        IMediator mediator,
        IEmployeeAccessor employeeAccessor)
    {
        _mediator = mediator;
        _employeeAccessor = employeeAccessor;
    }

    [HttpGet("{requirementId:guid}")]
    [ProducesResponseType(200, Type = typeof(Grade))]
    public async Task<IActionResult> GetAsync(Guid requirementId)
    {
        return await _mediator
            .Send(new GetGradeQuery { EmployeeId = _employeeAccessor.EmployeeId, RequirementId = requirementId })
            .MatchAsync(g => Ok(g), e => e.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpGet("{employeeId:guid}/{requirementId:guid}")]
    [ProducesResponseType(200, Type = typeof(Grade))]
    public async Task<IActionResult> GetAsync(Guid employeeId, Guid requirementId)
    {
        return await _mediator
            .Send(new GetGradeQuery { EmployeeId = employeeId, RequirementId = requirementId })
            .MatchAsync(g => Ok(g), e => e.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpGet("kpi/{employeeId:guid}/{periodId:guid}")]
    [ProducesResponseType(200, Type = typeof(Grade))]
    public async Task<IActionResult> CalculateKpiAsync(Guid employeeId, Guid periodId)
    {
        return await _mediator
            .Send(new CalculateKpiQuery { EmployeeId = employeeId, PeriodId = periodId })
            .MatchAsync(g => Ok(g), e => e.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(Grade))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGradeRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MatchAsync(g => Ok(g), e => e.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPut]
    [ProducesResponseType(200, Type = typeof(Grade))]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateGradeRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MatchAsync(g => Ok(g), e => e.MapToActionResult());
    }
}
