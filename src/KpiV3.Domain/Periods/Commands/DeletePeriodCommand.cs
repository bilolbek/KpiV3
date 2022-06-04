using KpiV3.Domain.Periods.Ports;
using MediatR;

namespace KpiV3.Domain.Periods.Commands;

public record DeletePeriodCommand : IRequest<Result<IError>>
{
    public Guid PeriodId { get; set; }
}

public class DeletePeriodCommandHandler : IRequestHandler<DeletePeriodCommand, Result<IError>>
{
    private readonly IPeriodRepository _repository;

    public DeletePeriodCommandHandler(IPeriodRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeletePeriodCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.PeriodId);
    }
}
