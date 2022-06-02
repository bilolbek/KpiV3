using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data.Common;

namespace KpiV3.Infrastructure.Data.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddNpsql(this IServiceCollection services, IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services
            .AddScoped<DbConnection>(_ => new NpgsqlConnection(configuration.GetConnectionString("Default")))
            .AddTransient(sp => new Database(sp.GetRequiredService<DbConnection>()));
    }
}
