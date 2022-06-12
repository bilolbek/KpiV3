namespace KpiV3.Domain.Common.DataContracts;

public record DateRange
{
    public DateTimeOffset From { get; init; }
    public DateTimeOffset To { get; init; }

    public bool Includes(DateTimeOffset dateTime)
    {
        return From >= dateTime && dateTime <= To;
    }

    public bool Includes(DateRange otherRange)
    {
        return From >= otherRange.From && otherRange.To <= To;
    }
}