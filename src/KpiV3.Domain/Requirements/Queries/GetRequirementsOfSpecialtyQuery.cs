using KpiV3.Domain.Requirements.DataContracts;
using MediatR;

namespace KpiV3.Domain.Requirements.Queries;

public record GetRequirementsOfSpecialtyQuery : IRequest<List<SpecialtyRequirement>>
{
    public Guid SpecialtyId { get; init; }
    public Guid PeriodId { get; init; }
}

public class GetRequirementsOfSpecialtyQueryHandler : IRequestHandler<GetRequirementsOfSpecialtyQuery, List<SpecialtyRequirement>>
{
    private readonly KpiContext _db;

    public GetRequirementsOfSpecialtyQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<List<SpecialtyRequirement>> Handle(GetRequirementsOfSpecialtyQuery request, CancellationToken cancellationToken)
    {
        return await _db.Requirements
            .Where(r => r.SpecialtyId == request.SpecialtyId && r.PeriodPart.PeriodId == request.PeriodId)
            .Select(r => new SpecialtyRequirement
            {
                RequirementId = r.Id,
                IndicatorId = r.IndicatorId,
                IndicatorName = r.Indicator.Name,
                PeriodPartId = r.PeriodPartId,
                PeriodPartName = r.PeriodPart.Name,
                Weight = r.Weight,
                Note = r.Note,
            })
            .ToListAsync(cancellationToken);
    }
}
