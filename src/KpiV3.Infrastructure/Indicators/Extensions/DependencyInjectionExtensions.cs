using KpiV3.Domain.Indicators.Ports;
using KpiV3.Infrastructure.Indicators.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Indicators.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIndicatorAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<IIndicatorRepository, IndicatorRepository>();
    }
}
