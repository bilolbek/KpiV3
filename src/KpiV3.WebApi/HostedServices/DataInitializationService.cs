using KpiV3.Domain;
using KpiV3.Domain.Common.Ports;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Positions.DataContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Polly;
using System.Data.Common;
using System.Net.Sockets;

namespace KpiV3.WebApi.HostedServices;

public class DataInitializationService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DataInitializationService> _logger;
    private readonly DataInitializationServiceOptions _options;

    public DataInitializationService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<DataInitializationService> logger,
        IOptions<DataInitializationServiceOptions> options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        var policy = Policy
            .Handle<SocketException>()
            .Or<DbException>()
            .WaitAndRetryAsync(new TimeSpan[]
            {
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(4),
                TimeSpan.FromSeconds(8),
                TimeSpan.FromSeconds(16),
                TimeSpan.FromSeconds(32),
            });

        await policy.ExecuteAsync(async () =>
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();

            await InitializeAsync(scope.ServiceProvider);
        });
    }

    private async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<KpiContext>();
        var passwordGenerator = serviceProvider.GetRequiredService<IPasswordGenerator>();
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
        var guidProvider = serviceProvider.GetRequiredService<IGuidProvider>();
        var dateProvider = serviceProvider.GetRequiredService<IDateProvider>();

        await using var transaction = await db.Database.BeginTransactionAsync();

        if (await db.Employees.AnyAsync() && await db.Employees.AnyAsync())
        {
            return;
        }

        var password = passwordGenerator.Generate();

        db.Employees.Add(new Employee
        {
            Id = guidProvider.New(),
            Email = _options.Email,
            Name = new()
            {
                FirstName = _options.FirstName,
                LastName = _options.LastName,
                MiddleName = _options.MiddleName,
            },
            Position = new Position
            {
                Id = guidProvider.New(),
                Name = "Admin",
                Type = PositionType.Root,
            },

            PasswordHash = passwordHasher.Hash(password),

            RegisteredDate = dateProvider.Now(),
        });

        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        _logger.LogInformation(
            "Admin has been created... Email: {Email}. Password: {Password}",
            _options.Email,
            password);
    }
}


public class DataInitializationServiceOptions
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string MiddleName { get; set; } = default!;
}