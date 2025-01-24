using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SharedKernel.Wrappers;

public class Result<T> : Result, IResult<T>
{
    private readonly T? _data;

    protected internal Result(T data) : base() => _data = data;
    protected internal Result(Error error) : base(error) { }

    [NotNull]
    public T Data 
        => IsSuccess 
        ? _data! 
        : throw new InvalidOperationException("Cannot access failure result data.");

    public static implicit operator T(Result<T> result) => result.Data;
    public static implicit operator Result<T>(Error error) => new(error);
    public static implicit operator Result<T>(T value) => new(value);
}

public class Result : IResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error is null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; protected set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Error? Error { get; protected set; }

    protected internal Result(Error error) => Error = error;
    protected internal Result() { }

    public static implicit operator Result(Error error) => new(error);

    public static Result Success(string? message = default) 
        => new() { Message = message };
    public static Result<T> Success<T>(T data, string? message = default) 
        => new(data) { Message = message };

    public static Result Failure(Error error) => new(error);
    public static Result<T> Failure<T>(Error error) => new(error);

    /// <summary>
    /// Returns the first failure from the specified <paramref name="results"/>.
    /// If there is no failure, a success is returned.
    /// </summary>
    /// <param name="results"></param>
    /// <returns>
    /// The first failure from the specified <paramref name="results"/> array, or a success it does not exist.
    /// </returns>
    public static Result FirstFailureOrSuccess(params Result[] results)
    {
        foreach (var result in results)
        {
            if (!result.IsSuccess)
            {
                return result;
            }
        }

        return Success();
    }
}
