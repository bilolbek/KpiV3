using System.Runtime.CompilerServices;

namespace KpiV3.Domain.Common;

public static class Ensure
{
    public static Result<IError> That(bool expected, string cause)
    {
        if (expected)
        {
            return Result<IError>.Ok();
        }

        return Result<IError>.Fail(new InvalidInput(cause));
    }
}
