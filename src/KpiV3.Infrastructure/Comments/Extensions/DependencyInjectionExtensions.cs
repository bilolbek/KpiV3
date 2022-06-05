using KpiV3.Domain.Comments.Ports;
using KpiV3.Infrastructure.Comments.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KpiV3.Infrastructure.Comments.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCommentAdapters(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommentRepository, CommentRepository>()
            .AddTransient<ICommentBlockRepository, CommentBlockRepository>();
    }
}
