using KpiV3.Domain.Indicators.DataContracts;
using KpiV3.Domain.Indicators.Ports;
using MediatR;

namespace KpiV3.Domain.Indicators.Commands;

public record UpdateIndicatorCommand : IRequest<Result<Indicator, IError>>
{
    public Guid IndicatorId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Comment { get; set; } = default!;
}

public class UpdateIndicatorCommandHandler : IRequestHandler<UpdateIndicatorCommand, Result<Indicator, IError>>
{
    private readonly IIndicatorRepository _repository;

    public UpdateIndicatorCommandHandler(IIndicatorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Indicator, IError>> Handle(UpdateIndicatorCommand request, CancellationToken cancellationToken)
    {
        var indicator = new Indicator
        {
            Id = request.IndicatorId,
            Name = request.Name,
            Description = request.Description,
            Comment = request.Comment,
        };

        return await _repository
            .UpdateAsync(indicator)
            .InsertSuccessAsync(() => indicator);
    }
}
