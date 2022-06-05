using KpiV3.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Common.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPrimitiveProviders(this IServiceCollection services)
    {
        return services
            .AddTransient<IGuidProvider, GuidProvider>()
            .AddTransient<IDateProvider, DateProvider>();
    }
}
