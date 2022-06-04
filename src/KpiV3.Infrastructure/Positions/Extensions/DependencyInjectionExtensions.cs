using KpiV3.Domain.Positions.Ports;
using KpiV3.Infrastructure.Positions.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Infrastructure.Positions.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPositionAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<IPositionRepository, PositionRepository>();
    }
}
