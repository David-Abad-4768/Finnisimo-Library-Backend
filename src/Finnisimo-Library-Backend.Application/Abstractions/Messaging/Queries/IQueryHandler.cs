using MediatR;
using ErrorOr;

namespace Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, ErrorOr<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
