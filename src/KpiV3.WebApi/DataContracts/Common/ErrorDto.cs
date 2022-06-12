namespace KpiV3.WebApi.DataContracts.Common;

public class ErrorDto
{
    public ErrorDto(params string[] errors)
    {
        Errors = errors.ToList();
    }

    public List<string> Errors { get; }
}
