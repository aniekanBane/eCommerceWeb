using MediatR;
using SharedKernel.Wrappers;

namespace eCommerceWeb.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
