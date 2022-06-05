using KpiV3.Domain.Common;
using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Ports;
using MediatR;

namespace KpiV3.Domain.Periods.Commands;

public record CreatePeriodCommand : IRequest<Result<Period, IError>>
{
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}

public class CreatePeriodCommandHandler : IRequestHandler<CreatePeriodCommand, Result<Period, IError>>
{
    private readonly IPeriodRepository _repository;
    private readonly IGuidProvider _guidProvider;

    public CreatePeriodCommandHandler(
        IPeriodRepository repository,
        IGuidProvider guidProvider)
    {
        _repository = repository;
        _guidProvider = guidProvider;
    }

    public async Task<Result<Period, IError>> Handle(CreatePeriodCommand request, CancellationToken cancellationToken)
    {
        return await Ensure
            .That(request.From < request.To, "From date must be less than to")
            .InsertSuccess(() => new Period
            {
                Id = _guidProvider.New(),
                Name = request.Name,
                From = request.From,
                To = request.To
            })
            .BindAsync(period => _repository
                .InsertAsync(period)
                .InsertSuccessAsync(() => period));
    }
}
