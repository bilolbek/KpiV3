using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using MediatR;

namespace KpiV3.Domain.Positions.Commands;

public record UpdatePositionCommand : IRequest<Result<Position, IError>>
{
    public Guid PositionId { get; set; }
    public string Name { get; set; } = default!;
}

public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, Result<Position, IError>>
{
    private readonly IPositionRepository _repository;

    public UpdatePositionCommandHandler(IPositionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Position, IError>> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
    {
        return await _repository
            .FindByIdAsync(request.PositionId)
            .BindAsync(position => position.Type is PositionType.Root ?
                Result<Position, IError>.Fail(new BusinessRuleViolation("Cannot update root position")) :
                Result<Position, IError>.Ok(position))
            .MapAsync(position => position with { Name = request.Name })
            .BindAsync(position => _repository
                .UpdateAsync(position)
                .InsertSuccessAsync(() => position));
    }
}