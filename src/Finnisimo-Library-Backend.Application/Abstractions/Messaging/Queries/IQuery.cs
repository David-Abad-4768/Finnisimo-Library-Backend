using MediatR;
using ErrorOr;

namespace Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>> { }
