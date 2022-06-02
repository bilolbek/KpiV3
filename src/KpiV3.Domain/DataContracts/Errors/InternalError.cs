namespace KpiV3.Domain.DataContracts.Errors;

public class InternalError : IError
{
    public InternalError(Exception exception)
    {
        Exception = exception;
    }

    public Exception Exception { get; }
    public string Message => Exception.Message;
}
