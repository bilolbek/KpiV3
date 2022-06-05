using KpiV3.Domain.Common;
using KpiV3.Infrastructure.Data;

namespace KpiV3.Infrastructure.Common;

internal class TransactionProvider : ITransactionProvider
{
    private readonly Database _db;

    public TransactionProvider(Database db)
    {
        _db = db;
    }

    public async Task<Result<IError>> RunAsync(Func<Task<Result<IError>>> action)
    {
        return await _db.RunTransactionAsync(action);
    }
}
