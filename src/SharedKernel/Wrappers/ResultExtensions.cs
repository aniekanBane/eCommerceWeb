namespace SharedKernel.Wrappers;

public static class ResultExtensions
{
    /// <summary>
    /// Transforms a Result's type from a source type to a destination type.
    /// </summary>
    /// <typeparam name="TSource">The result containing type.</typeparam>
    /// <typeparam name="TDestination">The output result containing type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="bindFunc">A function to transform the source value to the destination type.</param>
    /// <returns>A new result contianing the transformed value or a failure result.</returns>
    public static Result<TDestination> Bind<TSource, TDestination>(
        this Result<TSource> result, 
        Func<TSource, Result<TDestination>> bindFunc)
        => result.IsSuccess ? bindFunc(result.Data) : Result.Failure<TDestination>(result.Error);
    
    public static Result<T> Bind<T>(this Result result, Func<Result, Result<T>> bindFunc)
        => result.IsSuccess ? bindFunc(result) : Result.Failure<T>(result.Error);

    public static Result Bind<T>(this Result<T> result, Func<T, Result> bindFunc) 
        => result.IsSuccess ? bindFunc(result.Data) : Result.Failure(result.Error);

    /// <summary>
    /// Ensures that the specified predicate is true, otherwise returns a failure result with the specified error.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="predicate">A function that defines the condition to be met.</param>
    /// <param name="error">The error to return if the condition is not met.</param>
    /// <returns>The original result if successful and the predicate is true; otherwise, a failure result.</returns>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if(!result.IsSuccess)
            return result;

        return predicate(result.Data) ? result : Result.Failure<T>(error);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="result"></param>
    /// <param name="mapFunc"></param>
    /// <returns></returns>
    public static Result<TDestination> Map<TSource, TDestination>(
        this Result<TSource> result,
        Func<TSource, TDestination> mapFunc)
        => result.IsSuccess ? mapFunc(result) : Result.Failure<TDestination>(result.Error);
    
    public static Result<T> Map<T>(this Result result, Func<T> mapFunc)
        => result.IsSuccess ? mapFunc() : Result.Failure<T>(result.Error);

    /// <summary>
    /// Matches the result against success and failure handlers.
    /// </summary>
    /// <typeparam name="T">The result containing type.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <returns>The result of the appropriate handler based on the success state of the .</returns>
    public static TResult Match<T, TResult>(
        this Result<T> result, 
        Func<T, TResult> onSuccess, 
        Func<Error, TResult> onFailure) 
        => result.IsSuccess ? onSuccess(result.Data) : onFailure(result.Error);

    public static TResult Match<TResult>(this Result result, Func<TResult> onSuccess, Func<Error, TResult> onFailure)
        => result.IsSuccess ? onSuccess(): onFailure(result.Error);
    
    /// <summary>
    /// Convert a <see cref="Result{T}"/> to a <see cref="PagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public static PagedResult<T> ToPagedResult<T>(this Result<IEnumerable<T>> result, Pagination pagination)
        => new(result.Data, pagination);
}
