namespace SharedKernel.Wrappers;

public interface IPagedResult<T> : IPagedResult, IResult<IEnumerable<T>>;

public interface IPagedResult : IResult
{
    Pagination Pagination { get; }
}
