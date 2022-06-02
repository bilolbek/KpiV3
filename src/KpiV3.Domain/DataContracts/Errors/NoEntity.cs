namespace KpiV3.Domain.DataContracts.Errors;

public class NoEntity : IError
{
    public NoEntity(Type entityType) : this($"{entityType.Name} is not found")
    {
    }

    public NoEntity(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
