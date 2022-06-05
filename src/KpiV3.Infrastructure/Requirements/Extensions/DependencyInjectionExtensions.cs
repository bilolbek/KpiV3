using KpiV3.Domain.Requirements.Ports;
using KpiV3.Infrastructure.Requirements.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Requirements.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRequirementAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<IRequirementRepository, RequirementRepository>();
    }
}
