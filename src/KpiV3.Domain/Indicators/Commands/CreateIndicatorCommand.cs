using KpiV3.Domain.Common;
using KpiV3.Domain.Indicators.DataContracts;
using KpiV3.Domain.Indicators.Ports;
using MediatR;

namespace KpiV3.Domain.Indicators.Commands;

public record CreateIndicatorCommand : IRequest<Result<Indicator, IError>>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Comment { get; set; } = default!;
}

public class CreateIndicatorCommandHandler : IRequestHandler<CreateIndicatorCommand, Result<Indicator, IError>>
{
    private readonly IIndicatorRepository _repository;
    private readonly IGuidProvider _guidProvider;

    public CreateIndicatorCommandHandler(
        IIndicatorRepository repository, 
        IGuidProvider guidProvider)
    {
        _repository = repository;
        _guidProvider = guidProvider;
    }

    public async Task<Result<Indicator, IError>> Handle(CreateIndicatorCommand request, CancellationToken cancellationToken)
    {
        var indicator = new Indicator
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            Description = request.Description,
            Comment = request.Comment,
        };

        return await _repository
            .InsertAsync(indicator)
            .InsertSuccessAsync(() => indicator);
    }
}
