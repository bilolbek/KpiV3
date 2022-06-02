using KpiV3.Domain.DataContracts.Errors;
using System.Data.Common;

namespace KpiV3.Infrastructure.Data;

internal class DatabaseError : IError
{
    public DatabaseError(DbException exception)
    {
        Exception = exception;
    }

    public DbException Exception { get; }

    public string Message => Exception.Message;
}
