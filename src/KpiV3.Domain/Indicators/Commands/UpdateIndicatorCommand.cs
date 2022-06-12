using KpiV3.Domain.Indicators.DataContracts;
using MediatR;

namespace KpiV3.Domain.Indicators.Commands;

public record UpdateIndicatorCommand : IRequest<Indicator>
{
    public Guid IndicatorId { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string? Comment { get; set; }
}

public class UpdateIndicatorCommandHandler : IRequestHandler<UpdateIndicatorCommand, Indicator>
{
    private readonly KpiContext _db;

    public UpdateIndicatorCommandHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Indicator> Handle(UpdateIndicatorCommand request, CancellationToken cancellationToken)
    {
        var indicator = await _db.Indicators
            .FindAsync(new object?[] { request.IndicatorId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        indicator.Name = request.Name;
        indicator.Description = request.Description;
        indicator.Comment = request.Comment;

        await _db.SaveChangesAsync(cancellationToken);

        return indicator;
    }
}
