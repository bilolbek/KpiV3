using KpiV3.Domain.Submissions.Repositories;
using KpiV3.Infrastructure.Submissions.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Submissions.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSubmissionAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<ISubmissionRepository, SubmissionRepository>();
    }
}
