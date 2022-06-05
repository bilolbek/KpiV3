using KpiV3.Domain.Common;
using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.PeriodParts.Ports;
using KpiV3.Domain.Periods.Ports;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Commands;

public record UpdatePeriodPartCommand : IRequest<Result<PeriodPart, IError>>
{
    public Guid PartId { get; set; }
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}

public class UpdatePeriodPartCommandHandler : IRequestHandler<UpdatePeriodPartCommand, Result<PeriodPart, IError>>
{
    private readonly IPeriodPartRepository _periodPartRepository;
    private readonly IPeriodRepository _periodRepository;

    public UpdatePeriodPartCommandHandler(
        IPeriodPartRepository periodPartRepository,
        IPeriodRepository periodRepository)
    {
        _periodPartRepository = periodPartRepository;
        _periodRepository = periodRepository;
    }

    public async Task<Result<PeriodPart, IError>> Handle(UpdatePeriodPartCommand request, CancellationToken cancellationToken)
    {
        return await _periodPartRepository
            .FindByIdAsync(request.PartId)
            .BindAsync(part => _periodRepository
                .FindByIdAsync(part.PeriodId)
                .BindAsync(period => Ensure.That(request.From >= period.From && request.To <= period.To, ""))
                .InsertSuccessAsync(() => part with
                {
                    Name = request.Name,
                    From = request.From,
                    To = request.To,
                })
                .BindAsync(part => _periodPartRepository
                    .UpdateAsync(part)
                    .InsertSuccessAsync(() => part)));
    }
}
