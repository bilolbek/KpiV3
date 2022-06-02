﻿using KpiV3.Domain.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Adapters.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPrimitiveProviders(this IServiceCollection services)
    {
        return services
            .AddTransient<IGuidProvider, GuidProvider>()
            .AddTransient<IDateProvider, DateProvider>();
    }
}
