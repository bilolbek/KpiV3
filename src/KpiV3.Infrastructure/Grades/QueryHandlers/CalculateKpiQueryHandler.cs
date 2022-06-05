using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Grades.Queries;
using KpiV3.Domain.Requirements.Queries;
using KpiV3.Domain.Specialties.Queries;
using KpiV3.Infrastructure.Data;
using MediatR;

namespace KpiV3.Infrastructure.Grades.QueryHandlers;

internal class CalculateKpiQueryHandler : IRequestHandler<CalculateKpiQuery, Result<KpiModel, IError>>
{
    private readonly Database _db;
    private readonly IMediator _mediator;

    public CalculateKpiQueryHandler(
        Database db,
        IMediator mediator)
    {
        _db = db;
        _mediator = mediator;
    }

    public async Task<Result<KpiModel, IError>> Handle(CalculateKpiQuery request, CancellationToken cancellationToken)
    {
        return await _mediator
            .Send(new GetChoosenSpecialtyQuery
            {
                EmployeeId = request.EmployeeId,
                PeriodId = request.PeriodId,
            }, cancellationToken)
            .BindAsync(specialty => _mediator.Send(new GetRequirementsQuery
            {
                PeriodId = request.PeriodId,
                SpecialtyId = specialty.Id
            }))
            .BindAsync(async requirements =>
            {
                var total = 0.0;

                foreach (var requirement in requirements)
                {
                    var result = await _mediator.Send(new GetGradeQuery
                    {
                        RequirementId = requirement.Id,
                        EmployeeId = request.EmployeeId
                    });

                    if (result.IsFailure && result.Failure is not NoEntity)
                    {
                        return Result<KpiModel, IError>.Fail(result.Failure);
                    }

                    if (result.IsFailure)
                    {
                        continue;
                    }

                    total += result.Success.Value;
                }

                return Result<KpiModel, IError>.Ok(new KpiModel
                {
                    Kpi = Math.Round(total / requirements.Sum(r => r.Weight), 2)
                });
            });
    }
}
