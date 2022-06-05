using KpiV3.Domain.Employees.DataContracts;
using MediatR;

namespace KpiV3.Domain.Employees.Queries;

public record GetProfileQuery : IRequest<Result<Profile, IError>>
{
    public Guid EmployeeId { get; set; }
}
