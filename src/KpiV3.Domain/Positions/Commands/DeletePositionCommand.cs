using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using MediatR;

namespace KpiV3.Domain.Positions.Commands;

public record DeletePositionCommand : IRequest<Result<IError>>
{
    public Guid PositionId { get; set; }
}

public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, Result<IError>>
{
    private readonly IPositionRepository _positionRepository;

    public DeletePositionCommandHandler(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public async Task<Result<IError>> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        return await _positionRepository
            .FindByIdAsync(request.PositionId)
            .BindAsync(position => position.Type is PositionType.Root ?
                Result<IError>.Fail(new BusinessRuleViolation("Root position cannot be deleted")) :
                Result<IError>.Ok())
            .BindAsync(() => _positionRepository.DeleteAsync(request.PositionId));
    }
}
