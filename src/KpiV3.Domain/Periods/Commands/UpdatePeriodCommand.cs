using KpiV3.Domain.Common;
using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Ports;
using MediatR;

namespace KpiV3.Domain.Periods.Commands;

public record UpdatePeriodCommand : IRequest<Result<Period, IError>>
{
    public Guid PeriodId { get; set; }
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}

public class UpdatePeriodCommandHandler : IRequestHandler<UpdatePeriodCommand, Result<Period, IError>>
{
    private readonly IPeriodRepository _repository;

    public UpdatePeriodCommandHandler(IPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Period, IError>> Handle(UpdatePeriodCommand request, CancellationToken cancellationToken)
    {
        return await Ensure
            .That(request.From < request.To, "From must be less than To")
            .InsertSuccess(() => new Period
            {
                Id = request.PeriodId,
                Name = request.Name,
                From = request.From,
                To = request.To,
            })
            .BindAsync(period => _repository
                .UpdateAsync(period)
                .InsertSuccessAsync(() => period));
    }
}
