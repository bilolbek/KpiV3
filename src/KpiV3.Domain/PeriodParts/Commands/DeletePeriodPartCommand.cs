using KpiV3.Domain.PeriodParts.Ports;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Commands;

public record DeletePeriodPartCommand : IRequest<Result<IError>>
{
    public Guid PartId { get; set; }
}

public class DeletePeriodPartCommandHandler : IRequestHandler<DeletePeriodPartCommand, Result<IError>>
{
    public readonly IPeriodPartRepository _repository;

    public DeletePeriodPartCommandHandler(IPeriodPartRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeletePeriodPartCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.PartId);
    }
}
