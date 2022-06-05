using KpiV3.Infrastructure.Common.Extensions;
using KpiV3.Infrastructure.Data.Extensions;
using KpiV3.Infrastructure.Employees.Extensions;
using KpiV3.Infrastructure.Indicators.Extensions;
using KpiV3.Infrastructure.Periods.Extensions;
using KpiV3.Infrastructure.Positions.Extensions;
using KpiV3.Infrastructure.Specialties.Extensions;
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
            .AddEmployeeAdapters(configuration, environment)
            .AddPositionAdapters()
            .AddPeriodAdapters()
            .AddSpecialtyAdapters()
            .AddIndicatorAdapters();
    }
}
