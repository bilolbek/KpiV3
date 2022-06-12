using KpiV3.Domain;
using Microsoft.EntityFrameworkCore;

namespace KpiV3.WebApi.HostedServices;

public class MigrationService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MigrationService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<KpiContext>();

        await db.Database.MigrateAsync(stoppingToken);
    }
}
