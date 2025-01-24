namespace SharedKernel.Wrappers;

public sealed record class Pagination
{
    /// <summary>
    /// Current page number.
    /// </summary>
    public int PageIndex { get; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Total number of items.
    /// </summary>
    public int TotalRecords { get; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Number of items on the current page size.
    /// </summary>
    public int CurrentPageSize { get; }

    /// <summary>
    /// Index of first item on the current page.
    /// </summary>
    public int CurrentStartIndex { get; }

    /// <summary>
    /// Index of the last item on the current page.
    /// </summary>
    public int CurrentEndIndex { get; }

    /// <summary>
    /// Indicates whether there is a next page available.
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    // <summary>
    /// Indicates whether there is a previous page available.
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Initializes a new <see cref="Pagination"/> instance.
    /// </summary>
    /// <param name="index">The current page number (1-based).</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="count">The total number of items across all pages.</param>
    public Pagination(int index, int size, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(index);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);

        PageIndex = index;
        PageSize = size;
        TotalRecords = count;
        TotalPages = (int)Math.Ceiling(count / (double)size);

        var skip = (index - 1) * size;
        var currentPageSize = Math.Min(size, count - skip);
        var currentStartIndex = Math.Min(count, skip + 1);

        CurrentPageSize = currentPageSize;
        CurrentStartIndex = currentStartIndex;
        CurrentEndIndex = Math.Min(count, currentStartIndex + currentPageSize - 1);
    }
}
