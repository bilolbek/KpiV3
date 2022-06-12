namespace KpiV3.Domain.Common.DataContracts;

public readonly struct Pagination
{
    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int PageIndex => PageNumber - 1;

    public int Offset => PageIndex * PageSize;

    public int Limit => PageSize;
}
