using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Specialties.Queries;

public record GetSpecialtyQuery : IRequest<Result<Specialty, IError>>
{
    public Guid SpecialtyId { get; set; }
}
