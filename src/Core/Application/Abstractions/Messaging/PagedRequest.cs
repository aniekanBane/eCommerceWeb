namespace eCommerceWeb.Application.Abstractions.Messaging;

public abstract class PagedRequest
{
    internal virtual int MaxPageSize => 50;
    internal virtual int DefaultPageSize { get; set; } = 10;

    public virtual int PageNumber { get; set; } = 1;
    public int PageSize 
    { 
        get => DefaultPageSize; 
        set => DefaultPageSize = Math.Min(value, MaxPageSize); 
    }
}
