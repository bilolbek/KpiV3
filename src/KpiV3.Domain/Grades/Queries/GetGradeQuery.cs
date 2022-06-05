using KpiV3.Domain.Grades.DataContracts;
using MediatR;

namespace KpiV3.Domain.Grades.Queries;

public record GetGradeQuery : IRequest<Result<Grade, IError>>
{
    public Guid EmployeeId { get; set; }
    public Guid RequirementId { get; set; }
}
