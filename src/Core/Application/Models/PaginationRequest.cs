namespace eCommerceWeb.Application.Models;

public abstract class PaginationRequest
{
    internal virtual int MaxPageSize { get; } = 50;
    internal virtual int DefaultPageSize { get; set; } = 10;

    public virtual int Index { get; set; } = 1;

    public int Size 
    { 
        get => DefaultPageSize;
        set => DefaultPageSize = Math.Min(value, MaxPageSize); 
    }

    public string? Search { get; set; }
}
