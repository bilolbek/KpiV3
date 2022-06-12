using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.Periods.DataContracts;
using MediatR;

namespace KpiV3.Domain.Periods.Commands;

public record CreatePeriodCommand : IRequest<Period>
{
    public string Name { get; init; } = default!;
    public DateRange Range { get; init; } = default!;
}

public class CreatePeriodCommandHandler : IRequestHandler<CreatePeriodCommand, Period>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;

    public CreatePeriodCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider)
    {
        _db = db;
        _guidProvider = guidProvider;
    }

    public async Task<Period> Handle(CreatePeriodCommand request, CancellationToken cancellationToken)
    {
        var period = new Period
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            Range = request.Range,
            PeriodParts = new List<PeriodPart>(),
        };

        _db.Periods.Add(period);

        await _db.SaveChangesAsync(cancellationToken);

        return period;
    }
}
