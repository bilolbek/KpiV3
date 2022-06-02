namespace KpiV3.Domain.UnitTests;

internal class TestError : IError
{
    public TestError(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
