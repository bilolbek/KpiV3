using KpiV3.Domain.Grades.Ports;
using KpiV3.Infrastructure.Grades.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Grades.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddGradeAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<IGradeRepository, GradeRepository>();
    }
}
