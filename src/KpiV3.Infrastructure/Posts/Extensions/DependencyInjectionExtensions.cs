using KpiV3.Domain.Posts.Ports;
using KpiV3.Infrastructure.Posts.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Posts.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPostAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<IPostRepository, PostRepository>();
    }
}
