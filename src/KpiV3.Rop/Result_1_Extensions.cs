namespace KpiV3.Rop;

public static class Result_1_Extensions
{
    public static Task<T> MatchAsync<TFailure, T>(
        this Result<TFailure> result,
        Func<Task<T>> onSuccess,
        Func<TFailure, Task<T>> onFailure)
    {
        return result.Match(onSuccess, onFailure);
    }

    public static Task<T> MatchAsync<TFailure, T>(
        this Result<TFailure> result,
        Func<T> onSuccess,
        Func<TFailure, Task<T>> onFailure)
    {
        return result.MatchAsync(() => Task.FromResult(onSuccess()), onFailure);
    }

    public static Task<T> MatchAsync<TFailure, T>(
        this Result<TFailure> result,
        Func<Task<T>> onSuccess,
        Func<TFailure, T> onFailure)
    {
        return result.MatchAsync(onSuccess, failure => Task.FromResult(onFailure(failure)));
    }

    public static async Task<T> MatchAsync<TFailure, T>(
        this Task<Result<TFailure>> task,
        Func<Task<T>> onSuccess,
        Func<TFailure, Task<T>> onFailure)
    {
        var result = await task;

        return await result.MatchAsync(onSuccess, onFailure);
    }

    public static async Task<T> MatchAsync<TFailure, T>(
        this Task<Result<TFailure>> task,
        Func<T> onSuccess,
        Func<TFailure, Task<T>> onFailure)
    {
        var result = await task;

        return await result.MatchAsync(onSuccess, onFailure);
    }

    public static async Task<T> MatchAsync<TFailure, T>(
        this Task<Result<TFailure>> task,
        Func<Task<T>> onSuccess,
        Func<TFailure, T> onFailure)
    {
        var result = await task;

        return await result.MatchAsync(onSuccess, onFailure);
    }

    public static async Task<T> MatchAsync<TFailure, T>(
        this Task<Result<TFailure>> task,
        Func<T> onSuccess,
        Func<TFailure, T> onFailure)
    {
        var result = await task;

        return result.Match(onSuccess, onFailure);
    }

    public static Task<Result<TFailure>> BindAsync<TFailure>(
        this Result<TFailure> result,
        Func<Task<Result<TFailure>>> binder)
    {
        return result.MatchAsync(
            binder,
            failure => Task.FromResult(Result<TFailure>.Fail(failure)));
    }

    public static async Task<Result<TFailure>> BindAsync<TFailure>(
        this Task<Result<TFailure>> task,
        Func<Task<Result<TFailure>>> binder)
    {
        var result = await task;

        return await result.BindAsync(binder);
    }

    public static async Task<Result<TFailure>> BindAsync<TFailure>(
        this Task<Result<TFailure>> task,
        Func<Result<TFailure>> binder)
    {
        var result = await task;

        return result.Bind(binder);
    }

    public static Task<Result<TSuccess, TFailure>> InsertSuccessAsync<TSuccess, TFailure>(
        this Result<TFailure> result,
        Func<Task<Result<TSuccess, TFailure>>> binder)
    {
        return result.Match(binder, failure => Task.FromResult(Result<TSuccess, TFailure>.Fail(failure)));
    }

    public static async Task<Result<TSuccess, TFailure>> InsertSuccessAsync<TSuccess, TFailure>(
        this Task<Result<TFailure>> task,
        Func<Task<Result<TSuccess, TFailure>>> binder)
    {
        var result = await task;

        return await result.InsertSuccessAsync(binder);
    }

    public static async Task<Result<TSuccess, TFailure>> InsertSuccessAsync<TSuccess, TFailure>(
        this Task<Result<TFailure>> task,
        Func<Result<TSuccess, TFailure>> binder)
    {
        var result = await task;

        return result.InsertSuccess(binder);
    }

    public static Task<Result<TSuccess, TFailure>> InsertSuccessAsync<TSuccess, TFailure>(
        this Result<TFailure> result,
        Func<Task<TSuccess>> onSuccess)
    {
        return result.Match(
            async () => Result<TSuccess, TFailure>.Ok(await onSuccess()),
            failure => Task.FromResult(Result<TSuccess, TFailure>.Fail(failure)));
    }

    public static async Task<Result<TSuccess, TFailure>> InsertSuccessAsync<TSuccess, TFailure>(
        this Task<Result<TFailure>> task,
        Func<Task<TSuccess>> onSuccess)
    {
        var result = await task;

        return await result.InsertSuccessAsync(onSuccess);
    }

    public static async Task<Result<TSuccess, TFailure>> InsertSuccessAsync<TSuccess, TFailure>(
        this Task<Result<TFailure>> task,
        Func<TSuccess> onSuccess)
    {
        var result = await task;

        return result.InsertSuccess(onSuccess);
    }

    public static Task<Result<T>> MapFailureAsync<TFailure, T>(
        this Result<TFailure> result,
        Func<TFailure, Task<T>> mapper)
    {
        return result.Match(
            () => Task.FromResult(Result<T>.Ok()),
            async failure => Result<T>.Fail(await mapper(failure)));
    }

    public static async Task<Result<T>> MapFailureAsync<TFailure, T>(
        this Task<Result<TFailure>> task,
        Func<TFailure, Task<T>> mapper)
    {
        var result = await task;

        return await result.MapFailureAsync(mapper);
    }

    public static async Task<Result<T>> MapFailureAsync<TFailure, T>(
        this Task<Result<TFailure>> task,
        Func<TFailure, T> mapper)
    {
        var result = await task;

        return result.MapFailure(mapper);
    }

    public static Task<Result<T>> BindFailureAsync<TFailure, T>(
        this Result<TFailure> result,
        Func<TFailure, Task<Result<T>>> binder)
    {
        return result.Match(
            () => Task.FromResult(Result<T>.Ok()),
            binder);
    }

    public static async Task<Result<T>> BindFailureAsync<TFailure, T>(
        this Task<Result<TFailure>> task,
        Func<TFailure, Task<Result<T>>> binder)
    {
        var result = await task;

        return await result.BindFailureAsync(binder);
    }

    public static async Task<Result<T>> BindFailureAsync<TFailure, T>(
        this Task<Result<TFailure>> task,
        Func<TFailure, Result<T>> binder)
    {
        var result = await task;

        return result.BindFailure(binder);
    }

    public static async Task<Result<T>> TeeAsync<T>(
        this Result<T> result,
        Func<Task> onSuccess)
    {
        await result.Match(() => onSuccess(), _ => Task.CompletedTask);

        return result;
    }

    public static async Task<Result<T>> TeeAsync<T>(
        this Task<Result<T>> task,
        Func<Task> onSuccess)
    {
        var result = await task;

        return await result.TeeAsync(onSuccess);
    }

    public static async Task<Result<T>> TeeAsync<T>(
        this Task<Result<T>> task,
        Action onSuccess)
    {
        var result = await task;

        return result.Tee(onSuccess);
    }
}
