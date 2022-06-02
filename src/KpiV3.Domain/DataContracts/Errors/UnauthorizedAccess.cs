namespace KpiV3.Domain.DataContracts.Errors;

public class UnauthorizedAccess : IError
{
    public UnauthorizedAccess(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
