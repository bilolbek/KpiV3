using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record ImportedEmployee
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string Position { get; set; } = default!;
}

public record ImportEmployeesCommand : IRequest<Result<IError>>
{
    public List<ImportedEmployee> Employees { get; init; } = default!;
}

public class ImportEmployeesCommandHandler : IRequestHandler<ImportEmployeesCommand, Result<IError>>
{
    private readonly IPositionRepository _positionRepository;
    private readonly IMediator _mediator;

    public ImportEmployeesCommandHandler(
        IPositionRepository positionRepository,
        IMediator mediator)
    {
        _positionRepository = positionRepository;
        _mediator = mediator;
    }

    public async Task<Result<IError>> Handle(ImportEmployeesCommand request, CancellationToken cancellationToken)
    {
        var context = new BulkContext(_positionRepository, _mediator);

        foreach (var employee in request.Employees)
        {
            var result = await context.RegisterEmployeeAsync(employee);

            if (result.IsFailure)
            {
                return result;
            }
        }

        return Result<IError>.Ok();
    }

    private class BulkContext
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IMediator _mediator;
        private readonly Dictionary<string, Guid> _positionIds;

        public BulkContext(
            IPositionRepository positionRepository,
            IMediator mediator)
        {
            _positionRepository = positionRepository;
            _mediator = mediator;
            _positionIds = new();
        }

        public async Task<Result<IError>> RegisterEmployeeAsync(ImportedEmployee employee)
        {
            return await GetPositionIdAsync(employee.Position)
                .BindAsync(positionId => _mediator.Send(new RegisterEmployeeCommand
                {
                    Email = employee.Email,

                    Name = new()
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        MiddleName = employee.MiddleName,
                    },

                    PositionId = positionId,
                }));
        }

        private async Task<Result<Guid, IError>> GetPositionIdAsync(string positionName)
        {
            if (_positionIds.TryGetValue(positionName, out var positionId))
            {
                return Result<Guid, IError>.Ok(positionId);
            }

            return await TryFetchPositionId(positionName)
                .BindFailureAsync(async error => error is NoEntity ?
                    await CreatePositionAndItsIdAsync(positionName) :
                    Result<Guid, IError>.Fail(error));
        }

        private async Task<Result<Guid, IError>> TryFetchPositionId(string positionName)
        {
            return await _positionRepository
                .FindByNameAsync(positionName)
                .TeeAsync(position => _positionIds[position.Name] = position.Id)
                .MapAsync(position => position.Id);
        }

        private async Task<Result<Guid, IError>> CreatePositionAndItsIdAsync(string positionName)
        {
            return await _mediator
                .Send(new CreatePositionCommand
                {
                    Name = positionName,
                    Type = PositionType.Employee
                })
                .TeeAsync(position => _positionIds[position.Name] = position.Id)
                .MapAsync(position => position.Id);
        }
    }
}