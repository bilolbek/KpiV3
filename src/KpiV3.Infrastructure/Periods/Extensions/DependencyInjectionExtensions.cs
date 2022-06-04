using KpiV3.Domain.Periods.Ports;
using KpiV3.Infrastructure.Periods.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Periods.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPeriodAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<IPeriodRepository, PeriodRepository>();
    }
}
