using MediatR;
using ErrorOr;

namespace Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

public interface ICommandHandler<TCommand>
    : IRequestHandler<TCommand, ErrorOr<None>>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, ErrorOr<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
