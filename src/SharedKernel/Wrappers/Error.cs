using System.Collections.Immutable;
using SharedKernel.Constants;

namespace SharedKernel.Wrappers;

public sealed record class Error
{
    private Error(ErrorType errorType, string message, params string[] details) 
    {
        ErrorType = errorType;
        Message = message;
        Details = [ .. details ];
    }

    public string Message { get; }
    public ErrorType ErrorType { get; }
    public ImmutableArray<string> Details { get; }

    public static Error Failure(string message) => new(ErrorType.Failure, message);

    public static Error Validation(params string[] errors) => 
        new(ErrorType.Validation, ErrorMessages.Common.ValidationError, errors);

    public static Error NotFound(string message) => new(ErrorType.NotFound, message);

    public static Error Conflict(string message) => new(ErrorType.Conflict, message);

    public static Error Problem() => new(ErrorType.Problem, ErrorMessages.Common.ServerError);

    public static Error Forbidden() => new(ErrorType.Forbidden, string.Empty);

    public static Error UnAuthorized() => new(ErrorType.Unauthorized, string.Empty);
}
