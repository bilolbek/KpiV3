namespace KpiV3.Domain.DataContracts.Errors;

public class InvalidInput : IError
{
    public InvalidInput(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
