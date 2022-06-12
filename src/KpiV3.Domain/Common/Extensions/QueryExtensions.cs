using KpiV3.Domain.Common.DataContracts;

namespace KpiV3.Domain.Common.Extensions;

public static class QueryExtensions
{
    public static async Task<Page<T>> ToPageAsync<T>(
        this IQueryable<T> query,
        Pagination pagination, 
        CancellationToken cancellationToken = default)
    {
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ToListAsync(cancellationToken);

        return new Page<T>(total, pagination, items);
    }
}
