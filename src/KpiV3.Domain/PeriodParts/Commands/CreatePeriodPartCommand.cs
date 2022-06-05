using KpiV3.Domain.Common;
using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.PeriodParts.Ports;
using KpiV3.Domain.Periods.Ports;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Commands;

public record CreatePeriodPartCommand : IRequest<Result<PeriodPart, IError>>
{
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public Guid PeriodId { get; set; }
}


public class CreatePeriodPartCommandHandler : IRequestHandler<CreatePeriodPartCommand, Result<PeriodPart, IError>>
{
    private readonly IPeriodPartRepository _periodPartRepository;
    private readonly IPeriodRepository _periodRepository;
    private readonly IGuidProvider _guidProvider;

    public CreatePeriodPartCommandHandler(
        IPeriodPartRepository periodPartRepository,
        IPeriodRepository periodRepository,
        IGuidProvider guidProvider)
    {
        _periodPartRepository = periodPartRepository;
        _periodRepository = periodRepository;
        _guidProvider = guidProvider;
    }

    public async Task<Result<PeriodPart, IError>> Handle(CreatePeriodPartCommand request, CancellationToken cancellationToken)
    {
        var part = new PeriodPart
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            From = request.From,
            To = request.To,
            PeriodId = request.PeriodId,
        };

        return await _periodRepository
            .FindByIdAsync(request.PeriodId)
            .BindAsync(period =>
                Ensure.That(request.From >= period.From && request.To <= period.To, "Period part must lie within parent period's time range"))
            .BindAsync(() => _periodPartRepository.InsertAsync(part))
            .InsertSuccessAsync(() => part);
    }
}
