using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.Domain.Employees.Commands;

public record BulkRegisterEmployee
{
    [Required]
    public string Email { get; init; } = default!;

    [Required]
    public string FirstName { get; init; } = default!;
    
    [Required]
    public string LastName { get; init; } = default!;
    
    public string? MiddleName { get; init; }
    
    [Required]
    public string Position { get; init; } = default!;
}

public record BulkRegisterEmployeesCommand : IRequest<Result<IError>>
{
    public List<BulkRegisterEmployee> Employees { get; init; } = default!;
}

public class BulkRegisterEmployeesCommandHandler : IRequestHandler<BulkRegisterEmployeesCommand, Result<IError>>
{
    private readonly IPositionRepository _positionRepository;
    private readonly IMediator _mediator;

    public BulkRegisterEmployeesCommandHandler(
        IPositionRepository positionRepository,
        IMediator mediator)
    {
        _positionRepository = positionRepository;
        _mediator = mediator;
    }

    public async Task<Result<IError>> Handle(BulkRegisterEmployeesCommand request, CancellationToken cancellationToken)
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

        public async Task<Result<IError>> RegisterEmployeeAsync(BulkRegisterEmployee employee)
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