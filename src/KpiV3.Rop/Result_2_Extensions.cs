namespace KpiV3.Rop;

public static class Result_2_Extensions
{
    public static Task<T> MatchAsync<TSuccess, TFailure, T>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, Task<T>> onSuccess,
        Func<TFailure, Task<T>> onFailure)
    {
        return result.Match(onSuccess, onFailure);
    }

    public static Task<T> MatchAsync<TSuccess, TFailure, T>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, Task<T>> onSuccess,
        Func<TFailure, T> onFailure)
    {
        return result.MatchAsync(onSuccess, failure => Task.FromResult(onFailure(failure)));
    }

    public static Task<T> MatchAsync<TSuccess, TFailure, T>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, T> onSuccess,
        Func<TFailure, Task<T>> onFailure)
    {
        return result.MatchAsync(success => Task.FromResult(onSuccess(success)), onFailure);
    }

    public static async Task<T> MatchAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Task<T>> onSuccess,
        Func<TFailure, Task<T>> onFailure)
    {
        var result = await task;

        return await result.MatchAsync(onSuccess, onFailure);
    }

    public static async Task<T> MatchAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Task<T>> onSuccess,
        Func<TFailure, T> onFailure)
    {
        var result = await task;

        return await result.MatchAsync(onSuccess, onFailure);
    }

    public static async Task<T> MatchAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, T> onSuccess,
        Func<TFailure, Task<T>> onFailure)
    {
        var result = await task;

        return await result.MatchAsync(onSuccess, onFailure);
    }

    public static async Task<T> MatchAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, T> onSuccess,
        Func<TFailure, T> onFailure)
    {
        var result = await task;

        return result.Match(onSuccess, onFailure);
    }

    public static Task<Result<T, TFailure>> MapAsync<TSuccess, TFailure, T>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, Task<T>> mapper)
    {
        return result.MatchAsync(
            async success => Result<T, TFailure>.Ok(await mapper(success)),
            failure => Result<T, TFailure>.Fail(failure));
    }

    public static async Task<Result<T, TFailure>> MapAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Task<T>> mapper)
    {
        var result = await task;

        return await result.MapAsync(mapper);
    }

    public static async Task<Result<T, TFailure>> MapAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, T> mapper)
    {
        var result = await task;

        return result.Map(mapper);
    }

    public static Task<Result<TSuccess, T>> MapFailureAsync<TSuccess, TFailure, T>(
        this Result<TSuccess, TFailure> result,
        Func<TFailure, Task<T>> mapper)
    {
        return result.MatchAsync(
            sucess => Result<TSuccess, T>.Ok(sucess),
            async failure => Result<TSuccess, T>.Fail(await mapper(failure)));
    }

    public static async Task<Result<TSuccess, T>> MapFailureAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TFailure, Task<T>> mapper)
    {
        var result = await task;

        return await result.MapFailureAsync(mapper);
    }

    public static async Task<Result<TSuccess, T>> MapFailureAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TFailure, T> mapper)
    {
        var result = await task;

        return result.MapFailure(mapper);
    }

    public static Task<Result<T, TFailure>> BindAsync<TSuccess, TFailure, T>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, Task<Result<T, TFailure>>> binder)
    {
        return result.Match(binder, failure => Task.FromResult(Result<T, TFailure>.Fail(failure)));
    }

    public static async Task<Result<T, TFailure>> BindAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Task<Result<T, TFailure>>> binder)
    {
        var result = await task;

        return await result.BindAsync(binder);
    }

    public static async Task<Result<T, TFailure>> BindAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Result<T, TFailure>> binder)
    {
        var result = await task;

        return result.Bind(binder);
    }

    public static Task<Result<TSuccess, T>> BindFailureAsync<TSuccess, TFailure, T>(
        this Result<TSuccess, TFailure> result,
        Func<TFailure, Task<Result<TSuccess, T>>> binder)
    {
        return result.Match(success => Task.FromResult(Result<TSuccess, T>.Ok(success)), binder);
    }

    public static async Task<Result<TSuccess, T>> BindFailureAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TFailure, Task<Result<TSuccess, T>>> binder)
    {
        var result = await task;

        return await result.BindFailureAsync(binder);
    }

    public static async Task<Result<TSuccess, T>> BindFailureAsync<TSuccess, TFailure, T>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TFailure, Result<TSuccess, T>> binder)
    {
        var result = await task;

        return result.BindFailure(binder);
    }

    public static Task<Result<TFailure>> BindAsync<TSuccess, TFailure>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, Task<Result<TFailure>>> binder)
    {
        return result.Match(binder, failure => Task.FromResult(Result<TFailure>.Fail(failure)));
    }

    public static async Task<Result<TFailure>> BindAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Task<Result<TFailure>>> binder)
    {
        var result = await task;

        return await result.BindAsync(binder);
    }

    public static async Task<Result<TFailure>> BindAsync<TSuccess, TFailure>(
         this Task<Result<TSuccess, TFailure>> task,
         Func<TSuccess, Result<TFailure>> binder)
    {
        var result = await task;

        return result.Bind(binder);
    }

    public static Task<Result<TFailure>> BindFailureAsync<TSuccess, TFailure>(
        this Result<TSuccess, TFailure> result,
        Func<TFailure, Task<Result<TFailure>>> binder)
    {
        return result.Match(_ => Task.FromResult(Result<TFailure>.Ok()), binder);
    }

    public static async Task<Result<TFailure>> BindFailureAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TFailure, Task<Result<TFailure>>> binder)
    {
        var result = await task;

        return await result.BindFailureAsync(binder);
    }

    public static async Task<Result<TFailure>> BindFailureAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TFailure, Result<TFailure>> binder)
    {
        var result = await task;

        return result.BindFailure(binder);
    }

    public static async Task<Result<TSuccess, TFailure>> TeeAsync<TSuccess, TFailure>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, Task> onSuccess)
    {
        await result.Match(onSuccess, failure => Task.CompletedTask);

        return result;
    }

    public static async Task<Result<TSuccess, TFailure>> TeeAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Task> onSuccess)
    {
        var result = await task;

        return await result.TeeAsync(onSuccess);
    }

    public static async Task<Result<TSuccess, TFailure>> TeeAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Action<TSuccess> onSuccess)
    {
        var result = await task;

        return result.Tee(onSuccess);
    }

    public static async Task<Result<TSuccess, TFailure>> TeeFailureAsync<TSuccess, TFailure>(
        this Result<TSuccess, TFailure> result,
        Func<TFailure, Task> onFailure)
    {
        await result.Match(_ => Task.CompletedTask, onFailure);

        return result;
    }

    public static async Task<Result<TSuccess, TFailure>> TeeFailureAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TFailure, Task> onFailure)
    {
        var result = await task;

        return await result.TeeFailureAsync(onFailure);
    }

    public static async Task<Result<TSuccess, TFailure>> TeeFailureAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Action<TFailure> onFailure)
    {
        var result = await task;

        return result.TeeFailure(onFailure);
    }

    public static async Task<Result<TSuccess, TFailure>> TeeEitherAsync<TSuccess, TFailure>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, Task> onSuccess,
        Func<TFailure, Task> onFailure)
    {
        await result.Match(onSuccess, onFailure);

        return result;
    }

    public static async Task<Result<TSuccess, TFailure>> TeeEitherAsync<TSuccess, TFailure>(
        this Result<TSuccess, TFailure> result,
        Action<TSuccess> onSuccess,
        Func<TFailure, Task> onFailure)
    {
        await result.Match(success =>
        {
            onSuccess(success);
            return Task.CompletedTask;
        }, onFailure);

        return result;
    }

    public static async Task<Result<TSuccess, TFailure>> TeeEitherAsync<TSuccess, TFailure>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, Task> onSuccess,
        Action<TFailure> onFailure)
    {
        await result.Match(onSuccess, failure =>
        {
            onFailure(failure);
            return Task.CompletedTask;
        });

        return result;
    }

    public static async Task<Result<TSuccess, TFailure>> TeeEitherAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Task> onSuccess,
        Func<TFailure, Task> onFailure)
    {
        var result = await task;

        return await result.TeeEitherAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TSuccess, TFailure>> TeeEitherAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Action<TSuccess> onSuccess,
        Func<TFailure, Task> onFailure)
    {
        var result = await task;

        return await result.TeeEitherAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TSuccess, TFailure>> TeeEitherAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Func<TSuccess, Task> onSuccess,
        Action<TFailure> onFailure)
    {
        var result = await task;

        return await result.TeeEitherAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TSuccess, TFailure>> TeeEitherAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> task,
        Action<TSuccess> onSuccess,
        Action<TFailure> onFailure)
    {
        var result = await task;

        return result.TeeEither(onSuccess, onFailure);
    }
}
