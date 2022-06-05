using KpiV3.Domain.PeriodParts.Ports;
using KpiV3.Infrastructure.PeriodParts.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.PeriodParts.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPeriodPartAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<IPeriodPartRepository, PeriodPartRepository>();
    }
}
