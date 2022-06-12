namespace KpiV3.Domain.Common.Exceptions;

public class BusinessLogicException : Exception
{
    public BusinessLogicException(string? message) : base(message)
    {
    }
}
