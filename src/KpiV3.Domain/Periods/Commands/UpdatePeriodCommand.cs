using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Periods.DataContracts;
using MediatR;

namespace KpiV3.Domain.Periods.Commands;

public record UpdatePeriodCommand : IRequest<Period>
{
    public Guid PeriodId { get; init; }
    public string Name { get; init; } = default!;
    public DateRange Range { get; init; } = default!;
}

public class UpdatePeriodCommandHandler : IRequestHandler<UpdatePeriodCommand, Period>
{
    private readonly KpiContext _db;

    public UpdatePeriodCommandHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Period> Handle(UpdatePeriodCommand request, CancellationToken cancellationToken)
    {
        var period = await _db.Periods
            .FindAsync(new object?[] { request.PeriodId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        period.Name = request.Name;
        period.Range = request.Range;

        return period;
    }
}
