using KpiV3.Domain.Common;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using MediatR;

namespace KpiV3.Domain.Positions.Commands;

public record CreatePositionCommand : IRequest<Result<Position, IError>>
{
    public string Name { get; init; } = default!;
    public PositionType Type { get; init; } = default!;
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
            Type = request.Type,
        };

        return await _positionRepository
            .InsertAsync(position)
            .InsertSuccessAsync(() => position);
    }
}
