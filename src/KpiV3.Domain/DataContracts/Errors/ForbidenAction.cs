namespace KpiV3.Domain.DataContracts.Errors;

public class ForbidenAction : IError
{
    public ForbidenAction(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
