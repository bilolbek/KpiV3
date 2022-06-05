namespace KpiV3.Domain.Common;

public interface ITransactionProvider
{
    Task<Result<IError>> RunAsync(Func<Task<Result<IError>>> action);
}
