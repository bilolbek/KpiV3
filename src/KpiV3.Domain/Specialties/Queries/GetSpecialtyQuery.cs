using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Specialties.Queries;

public record GetSpecialtyQuery : IRequest<Specialty>
{
    public Guid SpecialtyId { get; init; }
}

public class GetSpecialtyQueryHandler : IRequestHandler<GetSpecialtyQuery, Specialty>
{
    private readonly KpiContext _db;

    public GetSpecialtyQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Specialty> Handle(GetSpecialtyQuery request, CancellationToken cancellationToken)
    {
        return await _db.Specialties
            .FindAsync(new object?[] { request.SpecialtyId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();
    }
}
