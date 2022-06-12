namespace KpiV3.Domain.Common.DataContracts;

public record DateRange
{
    public DateTimeOffset From { get; init; }
    public DateTimeOffset To { get; init; }

    public bool Includes(DateTimeOffset dateTime)
    {
        return dateTime >= From && dateTime <= To;
    }

    public bool Includes(DateRange otherRange)
    {
        return Includes(otherRange.From) && Includes(otherRange.From);
    }
}