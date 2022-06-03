using Microsoft.Extensions.Options;

namespace KpiV3.WebApi.HostedServices.DataInitialization.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDataInitializationService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .Configure<DataInitializationServiceOptions>(configuration.GetSection("DataInitialization"))
            .AddTransient<IValidateOptions<DataInitializationServiceOptions>, DataInitializationServiceOptionsValidator>()
            .AddHostedService<DataInitializationService>();
    }
}
