using KpiV3.Domain.Requirements.DataContracts;
using MediatR;

namespace KpiV3.Domain.Requirements.Commands;

public record CreateRequirementCommand : IRequest<Requirement>
{
    public Guid SpecialtyId { get; init; }
    public Guid PeriodPartId { get; init; }
    public Guid IndicatorId { get; init; }
    public string? Note { get; init; }
    public double Weight { get; init; }
}

public class CreateRequirementCommandHandler : IRequestHandler<CreateRequirementCommand, Requirement>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;

    public CreateRequirementCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider)
    {
        _db = db;
        _guidProvider = guidProvider;
    }

    public async Task<Requirement> Handle(CreateRequirementCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        await EnsureTotalWeightOfRequirementsDoesNotExceed100Async(request, cancellationToken);

        return await CreateRequirementAsync(request, cancellationToken);
    }

    private async Task<Requirement> CreateRequirementAsync(CreateRequirementCommand request, CancellationToken cancellationToken)
    {
        var requirement = new Requirement
        {
            Id = _guidProvider.New(),
            IndicatorId = request.IndicatorId,
            PeriodPartId = request.PeriodPartId,
            SpecialtyId = request.SpecialtyId,
            Note = request.Note,
            Weight = request.Weight,
        };

        _db.Requirements.Add(requirement);

        await _db.SaveChangesAsync(cancellationToken);

        return requirement;
    }

    private async Task EnsureTotalWeightOfRequirementsDoesNotExceed100Async(
        CreateRequirementCommand request,
        CancellationToken cancellationToken)
    {
        var part = await _db.PeriodParts
            .FindAsync(new object?[] { request.PeriodPartId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        var totalWeight = await _db.Requirements
            .Where(p => p.PeriodPart.PeriodId == part.PeriodId)
            .Where(p => p.SpecialtyId == request.SpecialtyId)
            .SumAsync(p => p.Weight, cancellationToken);

        if (totalWeight > 100.0)
        {
            throw new BusinessLogicException("Total weight of requirements cannot be more than 100");
        }
    }
}
