using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.DataContracts;
using MediatR;

namespace KpiV3.Domain.Employees.Queries;

public record GetEmployeesQuery : IRequest<Result<Page<Profile>, IError>>
{
    public Guid? PositionId { get; set; }
    public Pagination Pagination { get; set; }
}
