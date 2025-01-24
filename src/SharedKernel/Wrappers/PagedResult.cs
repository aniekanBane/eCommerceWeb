using System.Text.Json.Serialization;

namespace SharedKernel.Wrappers;

public class PagedResult<T>(IEnumerable<T> data, Pagination pagination) 
    : Result<IEnumerable<T>>(data), IPagedResult<T>
{
    [JsonIgnore]
    public Pagination Pagination { get; } = pagination;
}
