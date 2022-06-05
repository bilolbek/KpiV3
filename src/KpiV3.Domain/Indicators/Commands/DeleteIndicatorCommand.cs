using KpiV3.Domain.Indicators.Ports;
using MediatR;

namespace KpiV3.Domain.Indicators.Commands;

public record DeleteIndicatorCommand : IRequest<Result<IError>>
{
    public Guid IndicatorId { get; set; }
}

public class DeleteIndicatorCommandHandler : IRequestHandler<DeleteIndicatorCommand, Result<IError>>
{
    private readonly IIndicatorRepository _repository;

    public DeleteIndicatorCommandHandler(IIndicatorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeleteIndicatorCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.IndicatorId);
    }
}
