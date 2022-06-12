namespace KpiV3.Domain.Common.Exceptions;

public class InvalidInputException : Exception
{
    public InvalidInputException(string message) : base(message)
    {
    }
}
