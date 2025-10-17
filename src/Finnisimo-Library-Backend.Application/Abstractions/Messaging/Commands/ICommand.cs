using MediatR;
using ErrorOr;

namespace Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

public interface ICommand : IRequest<ErrorOr<None>> { }

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>,
                                       IBaseCommand
{ }

public interface IBaseCommand { }

public record None;
