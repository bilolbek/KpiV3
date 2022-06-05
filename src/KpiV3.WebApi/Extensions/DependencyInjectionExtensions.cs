using KpiV3.Domain.Files;

namespace KpiV3.WebApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services.AddTransient<FileService>();
    }
}
