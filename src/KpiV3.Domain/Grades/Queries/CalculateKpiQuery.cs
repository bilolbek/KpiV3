using KpiV3.Domain.Grades.DataContracts;
using MediatR;

namespace KpiV3.Domain.Grades.Queries;

public record CalculateKpiQuery : IRequest<Result<KpiModel, IError>>
{
    public Guid EmployeeId { get; set; }
    public Guid PeriodId { get; set; }
}
