using KpiV3.Domain.Requirements.DataContracts;
using MediatR;

namespace KpiV3.Domain.Requirements.Commands;

public record UpdateRequirementCommand : IRequest<Requirement>
{
    public Guid RequirementId { get; init; }
    public string? Note { get; init; }
    public double Weight { get; init; }
}

public class UpdateRequirementCommandHandler : IRequestHandler<UpdateRequirementCommand, Requirement>
{
    private readonly KpiContext _db;

    public UpdateRequirementCommandHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Requirement> Handle(UpdateRequirementCommand request, CancellationToken cancellationToken)
    {
        var requirement = await _db.Requirements
            .FindAsync(new object?[] { request.RequirementId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        requirement.Weight = request.Weight;
        requirement.Note = request.Note;

        await EnsureTotalWeightOfRequirementsDoesNotExceed100Async(requirement, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        return requirement;
    }

    private async Task EnsureTotalWeightOfRequirementsDoesNotExceed100Async(
      Requirement requirement,
      CancellationToken cancellationToken)
    {
        var part = await _db.PeriodParts
            .FindAsync(new object?[] { requirement.PeriodPartId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        var totalWeight = await _db.Requirements
            .Where(p => p.PeriodPart.PeriodId == part.PeriodId)
            .Where(p => p.SpecialtyId == requirement.SpecialtyId)
            .SumAsync(p => p.Weight, cancellationToken);

        if (totalWeight > 100.0)
        {
            throw new BusinessLogicException("Total weight of requirements cannot be more than 100");
        }
    }
}
