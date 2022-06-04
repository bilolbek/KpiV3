using KpiV3.Domain.Specialties.Ports;
using KpiV3.Infrastructure.Specialties.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Specialties.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSpecialtyAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<ISpecialtyRepository, SpecialtyRepository>();
    }
}
