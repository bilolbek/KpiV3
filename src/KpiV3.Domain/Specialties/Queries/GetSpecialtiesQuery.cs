using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Specialties.Queries;

public record GetSpecialtiesQuery : IRequest<List<Specialty>>
{
    public Guid PositionId { get; init; }
}

public class GetSpecialtiesQueryHandler : IRequestHandler<GetSpecialtiesQuery, List<Specialty>>
{
    private readonly KpiContext _db;

    public GetSpecialtiesQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<List<Specialty>> Handle(GetSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        return await _db.Specialties
            .Where(s => s.PositionId == request.PositionId)
            .ToListAsync(cancellationToken);
    }
}
