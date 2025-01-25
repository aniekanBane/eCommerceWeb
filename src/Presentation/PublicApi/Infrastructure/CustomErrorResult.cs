using SharedKernel.Wrappers;
using IHttpResult = Microsoft.AspNetCore.Http.IResult;

namespace eCommerceWeb.PublicApi.Infrastructure;

public static class CustomErrorResult
{
    public static IHttpResult Problem(Result result)
    {
        return result.IsSuccess 
            ? throw new InvalidOperationException()
            : TypedResults.Problem
            ( 
                detail: result.Error.Message,
                statusCode: GetStatusCode(result.Error.ErrorCode),
                extensions: GetErrors(result.Error)
            );

        static int GetStatusCode(ErrorCode errorCode) => errorCode switch
        {
            ErrorCode.Invalid => StatusCodes.Status400BadRequest,
            ErrorCode.NotFound => StatusCodes.Status404NotFound,
            ErrorCode.Conflict => StatusCodes.Status409Conflict,
            ErrorCode.Forbidden => StatusCodes.Status403Forbidden,
            ErrorCode.Failure => StatusCodes.Status422UnprocessableEntity,
            ErrorCode.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorCode.Unavailable => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError,
        };

        static Dictionary<string, object?>? GetErrors(Error error)
        {
            return error.ErrorCode is not ErrorCode.Invalid 
                ? default
                : new Dictionary<string, object?> 
                { 
                    ["error"] = error.Details,
                };
        }
    }
}
