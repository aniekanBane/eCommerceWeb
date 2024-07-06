namespace SharedKernel.Wrappers;

public class PageInfo(int pageNumber, int pageSize, int totalRecords)
{
    public int PageNumber { get; private set; } = pageNumber;
    public int PageSize { get; private set; } = Math.Max(pageSize, 1);
    public int TotalRecords { get; private set; } = totalRecords;
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}
