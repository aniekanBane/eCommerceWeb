using MediatR;
using SharedKernel.Wrappers;

namespace eCommerceWeb.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>; 
