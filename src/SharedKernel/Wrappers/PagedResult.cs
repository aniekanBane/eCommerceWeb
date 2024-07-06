using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SharedKernel.Wrappers;

public sealed class PagedResult<T> : Result<ImmutableList<T>>
{
    [JsonIgnore]
    public PageInfo PageInfo { get; }
    [JsonIgnore]
    public int CurrentPageSize { get; }
    [JsonIgnore]
    public int CurrentStartIndex { get; }
    [JsonIgnore]
    public int CurrentEndIndex { get; }

    public PagedResult(ImmutableList<T> data, PageInfo pageInfo) : base(data, default, default) 
    {
        PageInfo = pageInfo;
        CurrentPageSize = data.Count;
        CurrentStartIndex = pageInfo.TotalRecords == 0 ? 0 : ((pageInfo.PageNumber - 1) * pageInfo.PageSize) + 1;
        CurrentEndIndex = pageInfo.TotalRecords == 0 ? 0 : CurrentStartIndex + CurrentPageSize - 1;
    }
}
