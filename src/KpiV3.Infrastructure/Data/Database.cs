using Dapper;
using KpiV3.Domain.DataContracts.Errors;
using KpiV3.Rop;
using System.Data;
using System.Data.Common;

namespace KpiV3.Infrastructure.Data;

internal class Database
{
    private readonly DbConnection _connection;

    public Database(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Result<IError>> RunTransactionAsync(Func<Task<Result<IError>>> action)
    {
        try
        {
            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            await using var transaction = await _connection.BeginTransactionAsync();

            return await action()
                .TeeAsync(() => transaction.CommitAsync())
                .TeeFailureAsync(_ => transaction.RollbackAsync());
        }
        catch (Exception exception)
        {
            return Result<IError>.Fail(MapToError(exception));
        }
    }

    public async Task<Result<IError>> ExecuteAsync(CommandDefinition command)
    {
        try
        {
            await _connection.ExecuteAsync(command);

            return Result<IError>.Ok();
        }
        catch (Exception exception)
        {
            return Result<IError>.Fail(MapToError(exception));
        }
    }

    public async Task<Result<IError>> ExecuteRequiredChangeAsync<TEntity>(CommandDefinition command)
    {
        try
        {
            var rowsAffected = await _connection.ExecuteAsync(command);

            if (rowsAffected is 0)
            {
                return Result<IError>.Fail(new NoEntity(typeof(TEntity)));
            }

            return Result<IError>.Ok();
        }
        catch (Exception exception)
        {
            return Result<IError>.Fail(MapToError(exception));
        }
    }

    public async Task<Result<IEnumerable<T>, IError>> QueryAsync<T>(CommandDefinition command)
    {
        try
        {
            var rows = await _connection.QueryAsync<T>(command);

            return Result<IEnumerable<T>, IError>.Ok(rows.ToList());
        }
        catch (Exception exception)
        {
            return Result<IEnumerable<T>, IError>.Fail(MapToError(exception));
        }
    }

    public async Task<Result<IEnumerable<T>, IError>> QueryAsync<T1, T2, T>(
        CommandDefinition command,
        Func<T1, T2, T> map,
        string splitOn)
    {
        try
        {

            var rows = await _connection.QueryAsync(command, map, splitOn: splitOn);

            return Result<IEnumerable<T>, IError>.Ok(rows.ToList());
        }
        catch (Exception exception)
        {
            return Result<IEnumerable<T>, IError>.Fail(MapToError(exception));
        }
    }

    public async Task<Result<T, IError>> QueryFirstAsync<T>(CommandDefinition command)
    {
        try
        {
            var row = await _connection.QueryFirstAsync<T>(command);

            return Result<T, IError>.Ok(row);
        }
        catch (InvalidOperationException)
        {
            return Result<T, IError>.Fail(new NoEntity(typeof(T)));
        }
        catch (Exception exception)
        {
            return Result<T, IError>.Fail(MapToError(exception));
        }
    }

    private static IError MapToError(Exception exception)
    {
        return exception switch
        {
            DbException dbException => new DatabaseError(dbException),

            _ => new InternalError(exception)
        };
    }
}
