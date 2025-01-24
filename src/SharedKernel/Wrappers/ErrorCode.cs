namespace SharedKernel.Wrappers;

/// <summary>
/// Represents the various types of errors that can occur in the application.
/// </summary>
public enum ErrorCode
{
    /// <summary>
    /// Indicates a bad request error, typically due to invalid input.
    /// </summary>
    Invalid = 400,

    Unauthorized = 401,

    Forbidden = 403,

    NotFound = 404,

    Conflict = 409,

    /// <summary>
    /// Indicates that the server understands the content type of the request entity, 
    /// and the syntax of the request entity is correct, but it was unable to process the contained instructions.
    /// </summary>
    Failure = 422,

    /// <summary>
    /// Indicates an internal server error or unexpected problem.
    /// </summary>
    Problem = 500,
    
    Unavailable = 503,
}
