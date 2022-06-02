using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Ports;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record CreatePositionCommand : IRequest<Result<Position, IError>>
{
    public string Name { get; init; } = default!;
}

public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, Result<Position, IError>>
{
    private readonly IGuidProvider _guidProvider;
    private readonly IPositionRepository _positionRepository;

    public CreatePositionCommandHandler(
        IGuidProvider guidProvider,
        IPositionRepository positionRepository)
    {
        _guidProvider = guidProvider;
        _positionRepository = positionRepository;
    }

    public async Task<Result<Position, IError>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = new Position
        {
            Id = _guidProvider.New(),
            Name = request.Name,
        };

        return await _positionRepository
            .InsertAsync(position)
            .InsertSuccessAsync(() => position);
    }
}
