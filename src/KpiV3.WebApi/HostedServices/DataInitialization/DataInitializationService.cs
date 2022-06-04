using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using MediatR;
using Microsoft.Extensions.Options;

namespace KpiV3.WebApi.HostedServices.DataInitialization;

public class DataInitializationService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly DataInitializationServiceOptions _options;
    private readonly ILogger<DataInitializationService> _logger;

    public DataInitializationService(
        IServiceScopeFactory serviceScopeFactory,
        IOptions<DataInitializationServiceOptions> options,
        ILogger<DataInitializationService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var positionRepository = scope.ServiceProvider.GetRequiredService<IPositionRepository>();
        var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();

        await InitializePositionsAsync()
            .BindAsync(InitializeEmployeesAsync)
            .TeeAsync(() => _logger.LogInformation("Initialization complete..."))
            .TeeFailureAsync(failure => _logger.LogCritical("Error occured during initialization. {Message}", failure.Message));

        async Task<Result<IError>> InitializePositionsAsync()
        {
            _logger.LogInformation("Initializing positions...");

            foreach (var position in _options.Positions)
            {
                var result = await ExecuteRetryAsync(() => InitializePositionAsync(position));

                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Result<IError>.Ok();
        }

        async Task<Result<IError>> InitializePositionAsync(InitialPosition position)
        {
            var result = await positionRepository
                .FindByNameAsync(position.Name)
                .BindFailureAsync(async failure => failure is NoEntity ?
                    await mediator.Send(position.ToCreateCommand()) :
                    Result<Position, IError>.Fail(failure));

            if (result.IsFailure)
            {
                return Result<IError>.Fail(result.Failure);
            }

            return Result<IError>.Ok();
        }

        async Task<Result<IError>> InitializeEmployeesAsync()
        {
            _logger.LogInformation("Initializing employees...");

            foreach (var employee in _options.Employees)
            {
                var result = await ExecuteRetryAsync(() => InitializeEmployeeAsync(employee));

                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Result<IError>.Ok();
        }

        async Task<Result<IError>> InitializeEmployeeAsync(InitialEmployee employee)
        {
            return await employeeRepository
                .FindByEmailAsync(employee.Email)
                .BindFailureAsync(async failure => failure is NoEntity ?
                    await positionRepository
                        .FindByNameAsync(employee.Position)
                        .BindAsync(position => mediator.Send(employee.ToCreateCommand(position.Id))) :
                    Result<IError>.Fail(failure));
        }

        async Task<Result<IError>> ExecuteRetryAsync(Func<Task<Result<IError>>> action)
        {
            int retries = 0;

            Result<IError> result;

            do
            {
                result = await action();

                if (result.IsSuccess)
                {
                    break;
                }

                await Task.Delay(_options.RetryWait, stoppingToken);

            } while (retries < _options.RetryCount);

            return result;
        }
    }
}