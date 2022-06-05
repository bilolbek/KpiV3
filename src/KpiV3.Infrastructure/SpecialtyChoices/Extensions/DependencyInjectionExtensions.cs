using KpiV3.Domain.SpecialtyChoices.Ports;
using KpiV3.Infrastructure.SpecialtyChoices.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.SpecialtyChoices.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSpecialtyChoiceAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<ISpecialtyChoiceRepository, SpecialtyChoiceRepository>();
    }
}
