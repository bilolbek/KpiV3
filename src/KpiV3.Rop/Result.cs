using System.Diagnostics.CodeAnalysis;

namespace KpiV3.Rop;

public readonly struct Result<TSuccess, TFailure>
{
    private readonly TSuccess? _success;
    private readonly TFailure? _failure;

    private Result(
        TSuccess? success,
        TFailure? failure,
        bool isSuccess)
    {
        _success = success;
        _failure = failure;

        IsSuccess = isSuccess;
    }

    [MemberNotNullWhen(true, nameof(_success))]
    [MemberNotNullWhen(false, nameof(_failure))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(_failure))]
    [MemberNotNullWhen(false, nameof(_success))]
    public bool IsFailure => !IsSuccess;

    public TSuccess Success
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Trying access Success property while result is Failed");
            }

            return _success;
        }
    }

    public TFailure Failure
    {
        get
        {
            if (!IsFailure)
            {
                throw new InvalidOperationException("Trying access Failure property while result is Succeed");
            }

            return _failure;
        }
    }

    public bool TryGetSuccess([NotNullWhen(true)] out TSuccess? success)
    {
        success = default;

        if (IsSuccess)
        {
            success = _success;
            return true;
        }

        return false;
    }

    public bool TryGetFailure([NotNullWhen(true)] out TFailure? failure)
    {
        failure = default;

        if (IsFailure)
        {
            failure = _failure;
            return true;
        }

        return false;
    }

    public T Match<T>(Func<TSuccess, T> onSuccess, Func<TFailure, T> onFailure)
    {
        return IsSuccess ? onSuccess(_success) : onFailure(_failure);
    }

    public Result<T, TFailure> Map<T>(Func<TSuccess, T> mapper)
    {
        return IsSuccess ?
            Result<T, TFailure>.Ok(mapper(_success)) :
            Result<T, TFailure>.Fail(_failure);
    }

    public Result<TSuccess, T> MapFailure<T>(Func<TFailure, T> mapper)
    {
        return IsFailure ?
            Result<TSuccess, T>.Fail(mapper(_failure)) :
            Result<TSuccess, T>.Ok(_success);
    }

    public Result<T, TFailure> Bind<T>(Func<TSuccess, Result<T, TFailure>> binder)
    {
        return IsSuccess ?
            binder(_success) :
            Result<T, TFailure>.Fail(_failure);
    }

    public Result<TSuccess, T> BindFailure<T>(Func<TFailure, Result<TSuccess, T>> binder)
    {
        return IsFailure ?
            binder(_failure) :
            Result<TSuccess, T>.Ok(_success);
    }

    public Result<TFailure> Bind(Func<TSuccess, Result<TFailure>> binder)
    {
        return IsSuccess ? binder(_success) : Result<TFailure>.Fail(_failure);
    }

    public Result<TFailure> BindFailure(Func<TFailure, Result<TFailure>> binder)
    {
        return IsFailure ? binder(_failure) : Result<TFailure>.Ok();
    }

    public Result<TSuccess, TFailure> Tee(Action<TSuccess> onSuccess)
    {
        if (IsSuccess)
        {
            onSuccess(_success);
        }

        return this;
    }

    public Result<TSuccess, TFailure> TeeFailure(Action<TFailure> onFailure)
    {
        if (IsFailure)
        {
            onFailure(_failure);
        }

        return this;
    }

    public Result<TSuccess, TFailure> TeeEither(Action<TSuccess> onSuccess, Action<TFailure> onFailure)
    {
        if (IsSuccess)
        {
            onSuccess(_success);
        }
        else
        {
            onFailure(_failure);
        }

        return this;
    }

    public static Result<TSuccess, TFailure> Ok(TSuccess success)
    {
        return new Result<TSuccess, TFailure>(success, default, true);
    }

    public static Result<TSuccess, TFailure> Fail(TFailure failure)
    {
        return new Result<TSuccess, TFailure>(default, failure, false);
    }
}

public readonly struct Result<TFailure>
{
    private readonly TFailure? _failure;

    private Result(TFailure failure)
    {
        _failure = failure;

        IsFailure = true;
    }

    [MemberNotNullWhen(true, nameof(_failure))]
    public bool IsFailure { get; }

    [MemberNotNullWhen(false, nameof(_failure))]
    public bool IsSuccess => !IsFailure;

    public TFailure Failure
    {
        get
        {
            if (!IsFailure)
            {
                throw new InvalidOperationException("Trying access Failure property while result is Succeed");
            }

            return _failure;
        }
    }

    public T Match<T>(Func<T> onSuccess, Func<TFailure, T> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(_failure);
    }

    public Result<TSuccess, TFailure> InsertSuccess<TSuccess>(Func<Result<TSuccess, TFailure>> success)
    {
        return IsSuccess ? success() : Result<TSuccess, TFailure>.Fail(_failure);
    }

    public Result<TSuccess, TFailure> InsertSuccess<TSuccess>(Func<TSuccess> onSuccess)
    {
        return IsSuccess ?
            Result<TSuccess, TFailure>.Ok(onSuccess()) :
            Result<TSuccess, TFailure>.Fail(_failure);
    }

    public Result<TFailure> Bind(Func<Result<TFailure>> binder)
    {
        return IsSuccess ? binder() : Result<TFailure>.Fail(_failure);
    }

    public Result<T> MapFailure<T>(Func<TFailure, T> mapper)
    {
        return IsFailure ?
            Result<T>.Fail(mapper(_failure)) :
            Result<T>.Ok();
    }

    public Result<T> BindFailure<T>(Func<TFailure, Result<T>> binder)
    {
        return IsFailure ?
            binder(_failure) :
            Result<T>.Ok();
    }

    public Result<TFailure> Tee(Action onSuccess)
    {
        if (IsSuccess)
        {
            onSuccess();
        }

        return this;
    }

    public Result<TFailure> TeeFailure(Action<TFailure> onFailure)
    {
        if (IsFailure)
        {
            onFailure(_failure);
        }

        return this;
    }

    public Result<TFailure> TeeEither(Action onSuccess, Action<TFailure> onFailure)
    {
        if (IsSuccess)
        {
            onSuccess();
        }
        else
        {
            onFailure(_failure);
        }

        return this;
    }

    public static Result<TFailure> Ok()
    {
        return new Result<TFailure>();
    }

    public static Result<TFailure> Fail(TFailure failure)
    {
        return new Result<TFailure>(failure);
    }
}