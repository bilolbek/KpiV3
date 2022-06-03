namespace KpiV3.WebApi.HostedServices.DataInitialization;

public class DataInitializationServiceOptions
{
    public List<InitialEmployee> Employees { get; set; } = default!;
    public List<InitialPosition> Positions { get; set; } = default!;

    public int RetryCount { get; set; }
    public TimeSpan RetryWait { get; set; }
}
