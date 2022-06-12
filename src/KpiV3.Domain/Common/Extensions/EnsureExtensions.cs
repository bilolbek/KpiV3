using KpiV3.Domain.Common.Exceptions;

namespace KpiV3.Domain.Common.Extensions;

public static class EnsureExtensions
{
    public static T EnsureFound<T>(this T? entity)
    {
        if (entity is null)
        {
            throw new EntityNotFoundException(typeof(T));
        }

        return entity;
    }

    public static async Task<T> EnsureFoundAsync<T>(this Task<T?> task)
    {
        var entity = await task;

        return entity.EnsureFound();
    }

    public static async ValueTask<T> EnsureFoundAsync<T>(this ValueTask<T?> task)
    {
        var entity = await task;

        return entity.EnsureFound();
    }
}