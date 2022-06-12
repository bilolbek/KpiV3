using KpiV3.Domain.Requirements.DataContracts;
using MediatR;

namespace KpiV3.Domain.Requirements.Queries;

public record GetRequirementQuery : IRequest<Requirement>
{
    public Guid RequirementId { get; init; }
}

public class GetRequirementQueryHandler : IRequestHandler<GetRequirementQuery, Requirement>
{

    private readonly KpiContext _db;

    public GetRequirementQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Requirement> Handle(GetRequirementQuery request, CancellationToken cancellationToken)
    {
        return await _db.Requirements
            .FindAsync(new object?[] { request.RequirementId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();
    }
}
