using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Specialties.Queries;

public record GetChoosenSpecialtyQuery : IRequest<Result<Specialty, IError>>
{
    public Guid EmployeeId { get; set; }
    public Guid PeriodId { get; set; }
}
