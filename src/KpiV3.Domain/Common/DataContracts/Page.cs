namespace KpiV3.Domain.Common.DataContracts;

public readonly struct Page<T>
{
    private readonly Pagination _pagination;

    public Page(
        int totalItems,
        Pagination pagination,
        IEnumerable<T> items)
    {
        _pagination = pagination;

        TotalItems = totalItems;
        Items = items;
    }

    public IEnumerable<T> Items { get; }

    public int TotalItems { get; }

    public int CurrentPage => _pagination.PageNumber;

    public int PageSize => _pagination.PageSize;

    public int TotalPages
    {
        get
        {
            return (int)Math.Ceiling((double)TotalItems / _pagination.PageSize);
        }
    }

    public Page<TOther> Map<TOther>(Func<T, TOther> mapper)
    {
        return new Page<TOther>(TotalItems, _pagination, Items.Select(mapper));
    }
}