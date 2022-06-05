using KpiV3.Domain.Files.Ports;

namespace KpiV3.WebApi.HostedServices.FileStorageInitialization;

public class FileStorageInitializationService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<FileStorageInitializationService> _logger;

    public FileStorageInitializationService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<FileStorageInitializationService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var storage = scope.ServiceProvider.GetRequiredService<IFileStorage>();
        var tries = 0;

        do
        {
            var result = await storage.InitAsync();

            if (result.IsSuccess)
            {
                return;
            }

            tries++;
            await Task.Delay(TimeSpan.FromSeconds(5));
        } while (tries < 5);

        _logger.LogCritical("Unable to initialize file storage");
    }
}
