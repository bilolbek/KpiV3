using KpiV3.WebApi.HostedServices.DataInitialization.Extensions;

namespace KpiV3.WebApi.HostedServices.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddHostedServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddDataInitializationService(configuration);
    }
}
