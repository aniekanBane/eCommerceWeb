using System.Reflection;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using SharedKernel.Wrappers;

namespace eCommerceWeb.Application.Pipeline.ExceptionHandlers;

internal sealed class ValidationExceptionHandler<TRequest, TResponse, TException>
    : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<TResponse>
    where TException : ValidationException
{
    public Task Handle(
        TRequest request, 
        TException exception,
        RequestExceptionHandlerState<TResponse> state, 
        CancellationToken cancellationToken)
    {
        var errorMessages = exception.Errors.Select(x => x.ErrorMessage).Distinct().ToArray();
        var error = Error.Validation(errorMessages);

        dynamic? response = null;

        if (typeof(TResponse).IsGenericType 
            && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(TResponse).GetGenericArguments().First();
            var failMethod = typeof(Result)
              .GetMethods(BindingFlags.Public | BindingFlags.Static)
              .SingleOrDefault(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod)
              ?.MakeGenericMethod(resultType);

            if (failMethod is not null)
            {
                response = failMethod.Invoke(null, [ error ]);
            }
        }
        else if (typeof(TResponse) == typeof(Result))
        {
            response = Result.Failure(error);
        }
        
        if (response is not null)
            state.SetHandled((TResponse)response);

        return Task.CompletedTask;
    }
}
