using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SharedKernel.Wrappers;

public class Result : IResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error is null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; protected set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Error? Error { get; protected set; }

    protected internal Result(string? message, Error? error)
    {
        Message = message;
        Error = error;
    }

    public static Result Success() => new(null, null);
    public static Result Success(string message) => new(message, null);
    public static Result<T> Success<T>(T data) => new(data, null, null);
    public static Result<T> Success<T>(T data, string message) => new(data, message, null);

    public static Result Failure(Error error) => new(null, error);
    public static Result<T> Failure<T>(Error error) => new(error);
}

public class Result<T> : Result, IResult<T>
{
    private readonly T? _data;

    public Result(T data, string? message, Error? error) : base(message, error) => _data = data;
    public Result(Error error) : base(null, error) { }

    [NotNull]
    public T Data => IsSuccess ? _data! : throw new InvalidOperationException("Cannot access failure result data.");

    public static implicit operator T(Result<T> result) => result.Data;
    public static implicit operator Result<T>(T? value) => 
        value is null ? Failure<T>(Error.Problem()) : Success(value);
}
