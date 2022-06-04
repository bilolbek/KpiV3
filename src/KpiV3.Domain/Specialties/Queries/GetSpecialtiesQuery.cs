using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Specialties.Queries;

public record GetSpecialtiesQuery : IRequest<Result<List<Specialty>, IError>>
{
    public Guid PositionId { get; set; }
}