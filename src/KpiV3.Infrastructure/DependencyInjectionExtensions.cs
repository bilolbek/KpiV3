using KpiV3.Infrastructure.Adapters.Extensions;
using KpiV3.Infrastructure.Data.Extensions;
using KpiV3.Infrastructure.Employees.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAdapters(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        return services
            .AddPrimitiveProviders()
            .AddNpsql(configuration)
            .AddEmployeeAdapters(configuration, environment);
    }
}
