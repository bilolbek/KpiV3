namespace KpiV3.Domain.Common.Exceptions;

public class ForbiddenActionException : Exception
{
    public ForbiddenActionException(string message) : base(message)
    {
    }
}
