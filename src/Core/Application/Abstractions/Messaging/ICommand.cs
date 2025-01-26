using MediatR;
using SharedKernel.Wrappers;

namespace eCommerceWeb.Application.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;

public interface ICommand : IRequest<Result>;
