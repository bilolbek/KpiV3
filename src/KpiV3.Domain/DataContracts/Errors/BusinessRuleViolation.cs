namespace KpiV3.Domain.DataContracts.Errors;

public class BusinessRuleViolation : IError
{
    public BusinessRuleViolation(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
