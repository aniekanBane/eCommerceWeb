using SharedKernel.Constants;

namespace SharedKernel.Wrappers;

public sealed record class Error
{
    internal Error(ErrorCode errorCode, string message, params string[] details) 
    {
        ErrorCode = errorCode;
        Message = message;
        Details = details;
    }

    public string Message { get; }
    public ErrorCode ErrorCode { get; }
    public IEnumerable<string> Details { get; }

    public static Error Conflict(string message) => new(ErrorCode.Conflict, message);
    public static Error Failure(string message) => new(ErrorCode.Failure, message);
    public static Error Forbidden() => new(ErrorCode.Forbidden, string.Empty);
    public static Error Invalid(string message) => new(ErrorCode.Invalid, message);
    
    public static Error Validation(params string[] errors) => 
        new(ErrorCode.Invalid, ErrorMessages.Common.ValidationError, errors);

    public static Error NotFound(string message) => new(ErrorCode.NotFound, message);
    public static Error Problem() => new(ErrorCode.Problem, ErrorMessages.Common.ServerError);
    public static Error UnAuthorized() => new(ErrorCode.Unauthorized, string.Empty);
    public static Error UnAvailible() => new(ErrorCode.Unavailable, string.Empty);
}
