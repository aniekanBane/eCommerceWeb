namespace SharedKernel.Wrappers;

public interface IResult<out T> : IResult
{
    T? Data { get; }
}

public interface IResult
{
    Error? Error { get; }
    bool IsSuccess { get; }
    string? Message { get; }
}
